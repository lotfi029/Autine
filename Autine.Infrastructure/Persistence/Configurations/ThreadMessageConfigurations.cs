namespace Autine.Infrastructure.Persistence.Configurations;

public class ThreadMessageConfigurations : IEntityTypeConfiguration<ThreadMessage>
{
    public void Configure(EntityTypeBuilder<ThreadMessage> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasOne(t => t.Message)
              .WithMany(u => u.ThreadMessages)
              .HasForeignKey(t => t.MessageId)
              .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(t => t.ThreadMember)
              .WithMany(u => u.Messages)
              .HasForeignKey(t => t.ThreadMemberId)
              .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(tm => new { tm.ThreadMemberId, tm.MessageId })
            .IsUnique();
    }
}
