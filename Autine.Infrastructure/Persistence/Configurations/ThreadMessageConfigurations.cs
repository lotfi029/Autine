//namespace Autine.Infrastructure.Persistence.Configurations;

//public class ThreadMessageConfigurations : IEntityTypeConfiguration<ThreadMessage>
//{
//    public void Configure(EntityTypeBuilder<ThreadMessage> builder)
//    {
//        builder.HasKey(m => m.Id);

//        builder.HasOne(t => t.Sender)
//              .WithMany(u=>u.ThreadMessages)
//              .HasForeignKey(t => t.SenderId)
//              .OnDelete(DeleteBehavior.Restrict);
//    }
//}
