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
        
        var result = await aIModelService.AddModelAsync(
            request.UserId, new(
                request.Request.Name,
                request.Request.Context,
                request.Request.Bio
                ), isAdmin.IsSuccess,
            cancellationToken);

        if (result.IsFailure)
            return result.Error;

        var modelId = await unitOfWork.Bots.AddAsync(new()
        {
            Name = request.Request.Name,
            Context = request.Request.Context,
            Bio = request.Request.Bio,
            IsPublic = isAdmin.IsSuccess,
            CreatedBy = request.UserId,
        }, cancellationToken);

        return modelId;
    }
}
