namespace Autine.Application.Features.Profiles.Commands;
public class UpdateProfileCommandHandler(
    IUserService userService, 
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateProfileCommand>
{
    public Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
