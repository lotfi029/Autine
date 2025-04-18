namespace Autine.Infrastructure.Persistence.Configurations;

public class BotMessageConfigurations : IEntityTypeConfiguration<BotMessage>
{
    public void Configure(EntityTypeBuilder<BotMessage> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(m => m.BotPatient)
          .WithMany(b => b.BotMessages)
          .HasForeignKey(m => m.BotPatientId)
          .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(m => m.Message)
            .WithMany(b => b.BotMessages)
            .HasForeignKey(e => e.MessageId)
            .OnDelete(DeleteBehavior.NoAction);


    }

}
