namespace Autine.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<PatientSupervisor>
{
    public void Configure(EntityTypeBuilder<PatientSupervisor> builder)
    {
        builder.HasKey(e => new { e.SupervisorId, e.PatientId });

        builder.HasOne<ApplicationUser>()
            .WithMany(e => e.Patients)
            .HasForeignKey(e => e.SupervisorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>()
            .WithMany(e => e.SupervisoredPatients)
            .HasForeignKey(e => e.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.IsSupervised)
            .HasDefaultValue(false);
    }
}
