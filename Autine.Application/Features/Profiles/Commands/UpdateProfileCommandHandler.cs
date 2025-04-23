namespace Autine.Application.Features.Profiles.Commands;
public class UpdateProfileCommandHandler(
    IUserService userService, 
    IUnitOfWork unitOfWork,
    IRoleService roleService,
    IAIAuthService aIAuthService) : ICommandHandler<UpdateProfileCommand>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var serverResult = await userService.UpdateProfileAsync(request.UserId, request.UpdateRequest, cancellationToken);

            if (!serverResult.IsSuccess)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return serverResult;
            }
            var role = await roleService.GetUserRoleAsync(request.UserId);


            var aiResult = await aIAuthService.UpdateUserAsync(
                request.UserId, 
                role.Value,
                new(serverResult.Value.email, serverResult.Value.username, serverResult.Value.password, serverResult.Value.fname, serverResult.Value.lname, serverResult.Value.dateofbirth, serverResult.Value.gender), 
                "String!23",
                cancellationToken);

            if (aiResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return aiResult.Error;
            }

            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success();
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.InternalServerError("Error", "An error occure while update info");
        }
    }
}
