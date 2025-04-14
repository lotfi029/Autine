

namespace Autine.Infrastructure.Repositories;
public class BotRepository(ApplicationDbContext context) : Repository<Bot>(context), IBotRepository
{
}
