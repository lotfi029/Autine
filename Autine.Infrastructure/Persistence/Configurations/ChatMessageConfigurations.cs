//namespace Autine.Infrastructure.Persistence.Configurations;

//internal class ChatMessageConfigurations : IEntityTypeConfiguration<ChatMessage>
//{
//    public void Configure(EntityTypeBuilder<ChatMessage> builder)
//    {
//        builder.HasKey(cm => cm.Id);

           
//        builder.Property(m => m.Message)
//                .HasMaxLength(5000)
//                .IsRequired();


//        builder.Property(m => m.SentDate).IsRequired();
//        builder.Property(m => m.DeliveredDate);
//        builder.Property(m => m.SeenDate);
//        builder.Property(m => m.Direction).HasMaxLength(50).IsRequired();
//    }
    
//}
    
