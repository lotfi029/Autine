namespace Autine.Infrastructure.Persistence.Configurations;

public class BotMessageConfigurations : IEntityTypeConfiguration<BotMessage>
{
    public void Configure(EntityTypeBuilder<BotMessage> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(m => m.BotPatient)
          .WithMany(b => b.BotMessages)
          .HasForeignKey(m => m.BotPatientId)
          .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(m => m.Message)
            .WithOne()
            .HasForeignKey<BotMessage>(e => e.MessageId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasIndex(e => new { e.BotPatientId, e.MessageId })
            .IsUnique();
    }

}
