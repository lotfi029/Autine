using Autine.Application.Contracts.Auths;
using Autine.Application.Contracts.Bots;

namespace Autine.Application.Mapster;
public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Bot bot, BotPatientsResponse botPatient), DetailedBotResponse>()
            .Map(dest => dest.Patients, src => src.botPatient)
            .Map(dest => dest, src => src.bot);

        TypeAdapterConfig<CreateSupervisorRequest, RegisterRequest>
            .NewConfig()
            // explicitly map each field, ignoring the extra SuperviorRole
            .Map(d => d.FirstName, s => s.FirstName)
            .Map(d => d.LastName, s => s.LastName)
            .Map(d => d.Email, s => s.Email)
            .Map(d => d.UserName, s => s.UserName)
            .Map(d => d.Password, s => s.Password)
            .Map(d => d.Gender, s => s.Gender)
            .Map(d => d.Bio, s => s.Bio)
            .Map(d => d.ProfilePic, s => s.ProfilePic)   // copy the IFormFile over
            .Map(d => d.Country, s => s.Country)
            .Map(d => d.City, s => s.City)
            .Map(d => d.DateOfBirth, s => s.DateOfBirth)
            .Compile();
    }
}
