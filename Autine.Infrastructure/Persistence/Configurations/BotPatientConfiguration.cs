//namespace Autine.Infrastructure.Persistence.Configurations;

//public class BotPatientConfiguration : IEntityTypeConfiguration<BotPatient>
//{
//    public void Configure(EntityTypeBuilder<BotPatient> builder)
//    {
//        builder.HasKey(c => c.Id);

//        builder.HasOne(bp => bp.Bot)
//               .WithMany(b => b.BotPatients)
//               .HasForeignKey(bp => bp.BotId);

//        builder.HasOne(pb => pb.Patient)
//                .WithMany(p =>p.BotPatients)
//                .HasForeignKey(pb => pb.PatientId);
//    }

//}
