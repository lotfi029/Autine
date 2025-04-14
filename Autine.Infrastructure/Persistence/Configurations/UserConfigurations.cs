namespace Autine.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(e => e.FirstName)
            .HasMaxLength(100);
        builder.Property(e => e.LastName)
            .HasMaxLength(100);

        builder.Property(e => e.Bio)
            .HasMaxLength(2500);

        builder.Property(e => e.Gender)
            .HasMaxLength(100);

        builder.Property(e => e.ProfilePicture)
            .HasMaxLength(1000);

        builder.Property(e => e.Country)
            .HasMaxLength(100);

        builder.Property(e => e.City)
            .HasMaxLength(100);


        var adminUser = new ApplicationUser
        {
            Id = DefaultUsers.Id,
            FirstName = DefaultUsers.FirstName,
            LastName = DefaultUsers.LastName,
            Bio = DefaultUsers.LastName,
            ProfilePicture = "none",
            Gender = "male",
            DateOfBirth = new DateTime(2025, 2, 27),
            Country = "Egypt",
            City = "Kafr elsheikh",
            UserName = DefaultUsers.UserName,
            NormalizedUserName = DefaultUsers.UserName.ToUpper(),
            Email = DefaultUsers.Email,
            NormalizedEmail = DefaultUsers.Email.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = DefaultUsers.SecurityStamp,
            ConcurrencyStamp = DefaultUsers.ConcurrencyStamp,
            PasswordHash = "AQAAAAIAAYagAAAAEBbWjL8coqX4W28rbExSdO9oxmhKHv6wM4FPUC7EA+NPus+zl7GH7agHyr/+5JzfJQ=="
        };

        builder.HasData(adminUser);
    }
}