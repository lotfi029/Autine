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
        
        builder.HasOne(t => t.Thread)
              .WithMany(u => u.Messages)
              .HasForeignKey(t => t.ThreadId)
              .OnDelete(DeleteBehavior.NoAction);
    }
}
