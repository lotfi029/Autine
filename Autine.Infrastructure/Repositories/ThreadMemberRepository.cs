
namespace Autine.Infrastructure.Repositories;
public class ThreadMemberRepository(ApplicationDbContext context) : Repository<ThreadMember>(context), IThreadMemberRepository
{
}
