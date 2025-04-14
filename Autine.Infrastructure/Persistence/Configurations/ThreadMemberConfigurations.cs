//namespace Autine.Infrastructure.Persistence.Configurations;

//public class ThreadMemberConfigurations : IEntityTypeConfiguration<ThreadMember>
//{
//    public void Configure(EntityTypeBuilder<ThreadMember> builder)
//    {
//        builder.HasKey(x => x.Id);

//        builder.HasOne(m => m.User)
//               .WithMany(u => u.ThreadMember)
//               .HasForeignKey(m => m.UserId);
//    }
//}
