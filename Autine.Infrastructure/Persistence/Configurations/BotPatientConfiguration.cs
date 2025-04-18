namespace Autine.Infrastructure.Persistence.Configurations;

public class BotPatientConfiguration : IEntityTypeConfiguration<BotPatient>
{
    public void Configure(EntityTypeBuilder<BotPatient> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasOne(bp => bp.Bot)
               .WithMany(b => b.BotPatients)
               .HasForeignKey(bp => bp.BotId);

        builder.HasOne<ApplicationUser>()
               .WithMany(p => p.BotPatients)
               .HasForeignKey(pb => pb.PatientId);
    }
}
