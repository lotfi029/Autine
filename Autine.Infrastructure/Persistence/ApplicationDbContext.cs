using Autine.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Autine.Infrastructure.Persistence;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<PatientSupervisor> Patients { get; set; }
    //public DbSet<ChatMessage> ChatMessage { get; set; }
    //public DbSet<Chat> Chat { get; set; }
    //public DbSet<ThreadMessage> ThreadMessages { get; set; }
    //public DbSet<ChatThread> ChatThreads { get; set; }
    //public DbSet<ThreadMember> ThreadMembers { get; set; }
    //public DbSet<Bot> Bots { get; set; }
    //public DbSet<BotPatient> BotPatients { get; set; }
    //public DbSet<BotMessage> BotMessages { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UserConfigurations).Assembly);

        var cascadeFKs = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(e => e.DeleteBehavior == DeleteBehavior.Cascade && !e.IsOwnership);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(builder);
    }
}
