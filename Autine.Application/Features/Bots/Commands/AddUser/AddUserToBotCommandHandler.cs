namespace Autine.Application.Features.Bots.Commands.AddUser;
public class AddUserToBotCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<AddUserToBotCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddUserToBotCommand request, CancellationToken cancellationToken)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var botPatient = new BotPatient
            {
                BotId = request.BotId,
                UserId = request.UserId,
                IsUser = true,
            };            
            await unitOfWork.BotPatients.AddAsync(botPatient, cancellationToken);
            

            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success(botPatient.Id);
        }
        catch
        {
            // TODO: log error
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.BadRequest("AddUserToBot.Error", "an error occure while adding to user to bot.");
        }
    }
}
