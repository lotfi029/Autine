//using Autine.Application.Contracts.UserBots;

//namespace Autine.Application.Features.UserBots.Commands.Send;
//public class SendMessageToBotCommandHandler(
//    IUnitOfWork unitOfWork,
//    IAIModelService aIModelService,
//    IRoleService roleService
//    ) : ICommandHandler<SendMessageToBotCommand, MessageResponse>
//{
//    public async Task<Result<MessageResponse>> Handle(SendMessageToBotCommand request, CancellationToken cancellationToken)
//    {
//        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
//        try
//        {
//            // ensure botPatient
//            var isPatient = await roleService.IsInRoleAsync(request.UserId, "patient", cancellationToken);
//            if (isPatient.IsSuccess)
//            {

//            }
//            // message 
//            var userMessage = new Message()
//            {
//                SenderId = request.UserId,
//                Content = request.Content,
//                CreatedDate = DateTime.UtcNow,
//                DeliveredAt = DateTime.UtcNow,
//                ReadAt = DateTime.UtcNow,
//                Status = MessageStatus.Read
//            };
            
            


//            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);

//        }
//        catch
//        {
//            // TODO: log error
//            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
//            return Error.BadRequest("SendMessage.Error", "Error occure while sendimg message");
//        }

        
//    }
//}
  