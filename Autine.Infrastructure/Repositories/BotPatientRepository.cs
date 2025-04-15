namespace Autine.Infrastructure.Repositories;
public class BotPatientRepository(ApplicationDbContext context) : Repository<BotPatient>(context), IBotPatientRepository
{    
}
