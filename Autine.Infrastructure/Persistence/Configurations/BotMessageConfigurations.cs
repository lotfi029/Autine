//namespace Autine.Infrastructure.Persistence.Configurations;

//public class BotMessageConfigurations : IEntityTypeConfiguration<BotMessage>
//{
//    public void Configure(EntityTypeBuilder<BotMessage> builder)
//    {
//        builder.HasKey(c => c.Id);


//        builder.Property(b => b.Message)
//           .IsRequired()
//           .HasMaxLength(100);

//        builder.Property(m => m.SentDate)
//            .IsRequired();

//        builder.Property(m => m.Direction)
//            .HasMaxLength(50).IsRequired();

//        builder.HasOne(m => m.BotPatient)
//          .WithMany(b => b.BotMessages)
//          .HasForeignKey(m => m.BotPatientId)
//          .OnDelete(DeleteBehavior.Restrict);


//    }

//}
