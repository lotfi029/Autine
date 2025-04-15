using Autine.Application.Contracts.Patient;

namespace Autine.Application.Interfaces;
public interface IUserService
{
    Task<bool> CheckUserExist(string userId, CancellationToken ct = default);
    Task<IEnumerable<PatientResponse>> GetPatientsAsync(string userId, CancellationToken ct = default);
}
