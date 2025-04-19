namespace Autine.Infrastructure.Persistence.Configurations;

public class ThreadMemberConfigurations : IEntityTypeConfiguration<ThreadMember>
{
    public void Configure(EntityTypeBuilder<ThreadMember> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne<ApplicationUser>()
               .WithMany(u => u.ThreadMember)
               .HasForeignKey(m => m.MemberId);

        builder.HasOne(e => e.Patient)
               .WithMany(t => t.Members)
               .HasForeignKey(m => m.PatientId);

        builder.HasIndex(t => new { t.PatientId, t.MemberId })
            .IsUnique();
    }
}
