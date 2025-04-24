namespace Autine.Infrastructure.Persistence.Configurations;

public class BotPatientConfiguration : IEntityTypeConfiguration<BotPatient>
{
    public void Configure(EntityTypeBuilder<BotPatient> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasOne(bp => bp.Bot)
               .WithMany(b => b.BotPatients)
               .HasForeignKey(bp => bp.BotId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Patient)
               .WithMany(p => p.Bots)
               .HasForeignKey(pb => pb.PatientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<ApplicationUser>()
               .WithMany(e => e.BotUsers)
               .HasForeignKey(pb => pb.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => new { b.BotId, b.PatientId })
            .IsUnique();
    }
}
