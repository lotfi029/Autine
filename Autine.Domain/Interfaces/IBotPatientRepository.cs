namespace Autine.Domain.Interfaces;
public interface IBotPatientRepository : IRepository<BotPatient>
{
    Task<IEnumerable<BotMessage>> GetMessagesAsync(Guid botPatientId, CancellationToken ct = default);
    
}
