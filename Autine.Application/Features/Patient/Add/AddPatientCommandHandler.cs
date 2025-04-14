using Autine.Application.Interfaces.AIApi;
using Autine.Domain.Interfaces;

namespace Autine.Application.Features.Patient.Add;
public class AddPatientCommandHandler(
    IUnitOfWork unitOfWork, 
    IAuthService authService, 
    IAIAuthService aIAuthService) : ICommandHandler<AddPatientCommand>
{
    public async Task<Result> Handle(AddPatientCommand request, CancellationToken ct)
    {

        var aIResult = await aIAuthService.AddPatientAsync(
            request.UserId, new(
                request.Request.Email,
                request.Request.UserName,
                request.Request.Password,
                request.Request.FirstName,
                request.Request.LastName,
                request.Request.DateOfBirth,
                request.Request.Gender
                ), ct);

        if (aIResult.IsFailure)
            return aIResult.Error;

        var authResult = await authService.RegisterPatient(request.Request, ct);
        if (authResult.IsFailure)
            return authResult.Error;
        
        
        if (await unitOfWork.Patients.CheckExistAsync(e => e.SupervisorId == request.UserId && e.PatientId == authResult.Value, ct))
            return Error.Conflict("Patient.DuplicatedPatient", "this patient is already supervised by you.");

        await unitOfWork.Patients.Add(
            new()
            {
                IsSupervised = true,
                SupervisorId = request.UserId,
                PatientId = authResult.Value,
            }, ct);

        await unitOfWork.CommitChangesAsync(ct);

        return Result.Success();
    }
}
