using Autine.Application.Interfaces.AIApi;
using Autine.Domain.Interfaces;

namespace Autine.Application.Features.Bot.CreateBot;
public class CreateBotCommandHanlder(
    IUnitOfWork unitOfWork, 
    IAIModelService modelService) : ICommandHandler<CreateBotCommand>
{
    public async Task<Result> Handle(CreateBotCommand request, CancellationToken cancellationToken)
    {

        var modelId = await unitOfWork.Bots.Add(new()
        {
            Name = request.Request.Name,
            Context = request.Request.Context,
            Bio = request.Request.Bio
        }, cancellationToken);
        

        await unitOfWork.CommitChangesAsync(cancellationToken);


        var result = await modelService.AddModelAsync(request.UserId, new(modelId.ToString(), request.Request.Context, request.Request.Bio), cancellationToken);
        
        if (result.IsFailure)
            return result.Error;

        return Result.Success();
    }
}
