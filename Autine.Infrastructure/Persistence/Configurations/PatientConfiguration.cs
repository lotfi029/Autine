namespace Autine.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(e => new { e.SupervisorId, e.PatientId });

        builder.HasOne<ApplicationUser>()
            .WithMany(e => e.Patients)
            .HasForeignKey(e => e.SupervisorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>()
            .WithMany(e => e.Supervisors)
            .HasForeignKey(e => e.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.IsSupervised)
            .HasDefaultValue(false);
    }
}
