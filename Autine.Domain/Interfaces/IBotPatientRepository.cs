using Autine.Domain.Abstractions;

namespace Autine.Domain.Interfaces;
public interface IBotPatientRepository : IRepository<BotPatient>
{
    Task<IEnumerable<BotMessage>> GetMessagesAsync(Guid botPatientId, CancellationToken ct = default);
    Task<Result> DeleteBotPatientAsync(BotPatient bot, CancellationToken ct = default);


}
