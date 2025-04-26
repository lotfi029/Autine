using Autine.Domain.Abstractions;

namespace Autine.Domain.Interfaces;

public interface IBotMessageRepository : IRepository<BotMessage>
{
    Task<Result> DeleteBotMessageWithRelationAsync(Guid botPatientId, CancellationToken ct = default);
}
