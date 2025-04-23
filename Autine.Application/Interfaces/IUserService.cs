using Autine.Application.Contracts.Bots;
using Autine.Application.Contracts.Patients;
using Autine.Application.Contracts.Profiles;

namespace Autine.Application.Interfaces;
public interface IUserService
{
    Task<bool> CheckUserExist(string userId, CancellationToken ct = default);
    
    Task<IEnumerable<PatientResponse>> GetPatientsAsync(string userId, bool isFollowing = false, CancellationToken ct = default);
    Task<PatientResponse?> GetPatientByIdAsync(string userId, Guid id, CancellationToken ct = default);
    Task<IEnumerable<BotPatientResponse>> GetBotPatientAsync(Guid botId, CancellationToken ct = default);

    Task<Result> UpdateUserRequest(string userId, UpdateUserRequest request, CancellationToken ct = default);
    Task<Result> DeleteUserAsync(string userId, CancellationToken ct = default);

    // get
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken ct = default);
    // put
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken ct = default);
}
