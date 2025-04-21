namespace Autine.Application.Features.Bots.Commands.Create;
public class CreateBotCommandHanlder(
    IUnitOfWork unitOfWork, 
    IRoleService roleService,
    IAIModelService aIModelService) : ICommandHandler<CreateBotCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBotCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Bots.CheckExistAsync(e => e.Name == request.Request.Name, cancellationToken))
            return BotErrors.DuplicatedBot;

        var isAdmin = await roleService.UserIsAdminAsync(request.UserId);

        if (isAdmin.IsSuccess)
        {
            if (request.Request.PatientIds is not null)
                return AdminErrors.InvalidRole;
        }
        
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {

            var modelId = await unitOfWork.Bots.AddAsync(new()
            {
                Name = request.Request.Name,
                Context = request.Request.Context,
                Bio = request.Request.Bio,
                IsPublic = isAdmin.IsSuccess,
                CreatedBy = request.UserId
            }, cancellationToken);

            var result = await aIModelService.AddModelAsync(
            request.UserId, new(
                request.Request.Name,
                request.Request.Context,
                request.Request.Bio
                ), isAdmin.IsSuccess,
            cancellationToken);

            if (result.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return result.Error;
            }
            var ids = request.Request.PatientIds;


            if (ids is not null)
            {
                var patients = await unitOfWork.Patients.ArePatientsAsync(ids, ct: cancellationToken);

                if (patients is null || !patients.Any())
                    return PatientErrors.InvalidPatients;
                var p = patients.ToList();
                var botPatient = new List<BotPatient>();
                var count = p.Count;
                for (int i = 0; i < count; i++)
                {
                    var aiResult = await aIModelService.AssignModelAsync(
                        request.UserId,
                        request.Request.Name,
                        p[i].PatientId,
                        cancellationToken);

                    if (aiResult.IsFailure)
                    {
                        for(var j = i-1; j >= 0; j--)
                        {
                            await aIModelService.DeleteAssignAsync(
                                request.UserId,
                                p[j].PatientId,
                                request.Request.Name,
                                cancellationToken);
                        }

                        await aIModelService.RemoveModelAsync(
                            request.UserId, 
                            request.Request.Name, 
                            isAdmin.IsSuccess, 
                            cancellationToken);

                        await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);

                        return aiResult.Error;
                    }

                    botPatient.Add(new()
                    {
                        BotId = modelId,
                        PatientId = p[i].Id
                    });
                }

                await unitOfWork.BotPatients.AddRangeAsync(botPatient, cancellationToken);
                await unitOfWork.CommitChangesAsync(cancellationToken);
            }
            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success(modelId);
        }
        catch
        {
            // TODO: log error
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.BadRequest("Error", "an error occure");
        }
    }
}
