namespace Autine.Domain.Interfaces;
public interface IBotRepository : IRepository<Bot>
{
    Task DeleteBotAsync(Bot bot, CancellationToken ct = default);


}
