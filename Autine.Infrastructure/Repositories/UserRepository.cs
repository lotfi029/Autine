namespace Autine.Infrastructure.Repositories;
public class UserRepository(ApplicationDbContext context) : Repository<ApplicationUser>(context), IUserRepository<ApplicationUser>
{

}
