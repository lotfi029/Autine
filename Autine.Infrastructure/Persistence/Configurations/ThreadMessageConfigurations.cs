namespace Autine.Infrastructure.Persistence.Configurations;

public class ThreadMessageConfigurations : IEntityTypeConfiguration<ThreadMessage>
{
    public void Configure(EntityTypeBuilder<ThreadMessage> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasOne(t => t.Message)
              .WithOne()
              .HasForeignKey<ThreadMessage>(t => t.MessageId)
              .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t => t.ThreadMember)
              .WithMany(u => u.Messages)
              .HasForeignKey(t => t.ThreadMemberId)
              .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tm => new { tm.ThreadMemberId, tm.MessageId })
            .IsUnique();
    }
}
