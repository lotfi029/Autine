//namespace Autine.Infrastructure.Persistence.Configurations;

//public class ThreadConfigurations : IEntityTypeConfiguration<ChatThread>
//{
//    public void Configure(EntityTypeBuilder<ChatThread> builder)
//    {
//        builder.HasKey(t => t.Id);
//        builder.Property(t=>t.IsActive)
//            .HasDefaultValue(true);
//        builder.HasOne(t=>t.Doctor)
//               .WithMany(x=>x.OwnedThreads)
//               .HasForeignKey(t => t.OwnerId)
//               .OnDelete(DeleteBehavior.Restrict);
              
//        builder.HasOne(t => t.Patient)
//               .WithOne(x => x.PatientThread)
//               .HasForeignKey<ChatThread>(t=> t.PatientId)
//               .OnDelete(DeleteBehavior.Restrict);

//        builder.HasMany(t => t.Messages)
//               .WithOne(m => m.Thread)
//               .HasForeignKey(m => m.ThreadId);

//        builder.HasMany(t => t.Members)
//               .WithOne(m => m.Thread)
//               .HasForeignKey(m => m.ThreadId);

//    }
//}
