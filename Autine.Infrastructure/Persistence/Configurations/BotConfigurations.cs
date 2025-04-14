//namespace Autine.Infrastructure.Persistence.Configurations;

//public class BotConfigurations : IEntityTypeConfiguration<Bot>
//{
//    public void Configure(EntityTypeBuilder<Bot> builder)
//    {
//        builder.HasKey(c => c.Id);

//        builder.HasOne(b => b.Creator)
//           .WithMany(u => u.CreatedBots)
//           .HasForeignKey(b => b.CreatorId)
//           .OnDelete(DeleteBehavior.Restrict);


//        //builder.HasOne(b => b.Patient)
//        //  .WithMany(p=>p.PatientBots)
//        //  .HasForeignKey(b => b.PatientId)
//        //  .OnDelete(DeleteBehavior.Restrict);

//        builder.Property(b => b.Name)
//           .IsRequired()
//           .HasMaxLength(100);

//        builder.Property(b => b.Context)
//         .IsRequired();

//        builder.Property(b => b.Bio)
//           .IsRequired()
//           .HasMaxLength(1000);

//        builder.Property(b => b.CreatedAt)
//          .IsRequired();
//    }

//}
