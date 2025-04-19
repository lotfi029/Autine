namespace Autine.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne<ApplicationUser>()
            .WithMany(e => e.Patients)
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>()
            .WithMany(e => e.Supervisors)
            .HasForeignKey(e => e.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.IsSupervised)
            .HasDefaultValue(false);


        builder.HasIndex(p => new { p.PatientId, p.CreatedBy })
            .IsUnique();
    }
}
