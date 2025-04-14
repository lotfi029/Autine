//namespace Autine.Infrastructure.Persistence.Configurations;

//internal class ChatConfigurations : IEntityTypeConfiguration<Chat>
//{
//    public void Configure(EntityTypeBuilder<Chat> builder)
//    {
//         builder.HasKey(c => c.Id);
//        //builder.HasKey(c => new { c.Id, c.UserId });
//        builder.HasMany(c => c.Messages)
//            .WithOne(m => m.Chat)
//            //.HasForeignKey(m => new { m.ChatId, m.UserId })
//            .HasForeignKey(m => m.ChatId)
//            .IsRequired();

//        builder.HasOne(c => c.ApplicationUser)
//               .WithMany(cm => cm.UserChats)
//             .HasForeignKey(cm => cm.UserId)
//             .IsRequired();

//    }
//}

