namespace Autine.Infrastructure.Persistence.Configurations;

public class MessageConfigurations : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(m => m.Status)
            .HasConversion<int>();

        builder.Property(m => m.Content)
            .HasMaxLength(1000);

        builder.HasOne<ApplicationUser>()
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.SenderId);

    }

}
