using Autine.Application.Contracts.Bots;

namespace Autine.Application.Interfaces;

public interface IBotService
{
    Task<IEnumerable<PatientBotsResponse>> GetPatientBotsAsync(string userId, CancellationToken ct = default);
}

