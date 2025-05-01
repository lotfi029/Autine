namespace Autine.Infrastructure.Persistence.Configurations;

internal class ChatConfigurations : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(cm => cm.UserIdOne)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(cm => cm.UserIdTwo)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

