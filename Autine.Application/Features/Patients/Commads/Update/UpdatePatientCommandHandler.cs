using Autine.Application.Contracts.Profiles;
using Autine.Application.ExternalContracts.Auth;

namespace Autine.Application.Features.Patients.Commads.Update;
public class UpdatePatientCommandHandler(
    IAccountService accountService,
    IAIAuthService aIAuthService,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdatePatientCommand>
{
    public async Task<Result> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Patients
            .GetAsync(
            e => e.PatientId == request.PatientId && 
            e.IsSupervised && 
            e.CreatedBy == request.UserId, ct:cancellationToken) is not { } patient)
            return PatientErrors.PatientsNotFound;

        var updateProfileRequest = new UpdateUserProfileRequest(
            request.UpdateRequest.FirstName,
            request.UpdateRequest.LastName,
            request.UpdateRequest.Bio,
            request.UpdateRequest.Country,
            request.UpdateRequest.City
            );

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var userUpdateResult = await accountService.UpdateProfileAsync(patient.PatientId, updateProfileRequest, cancellationToken);
            
            if (userUpdateResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return userUpdateResult;
            }

            await unitOfWork.Patients.ExcuteUpdateAsync(
                e => e.Id == patient.Id,
                setters =>
                setters.SetProperty(e => e.NextSession, request.UpdateRequest.NextSession)
                .SetProperty(e => e.LastSession, request.UpdateRequest.LastSession)
                .SetProperty(e => e.Diagnosis, request.UpdateRequest.Diagnosis)
                .SetProperty(e => e.Status, request.UpdateRequest.Status)
                .SetProperty(e => e.Notes, request.UpdateRequest.Notes)
                .SetProperty(e => e.SessionFrequency, request.UpdateRequest.SessionFrequency),
                cancellationToken
                );
            var aiUpdateRequest = new AIUpdateRequest(
                userUpdateResult.Value.fname,
                userUpdateResult.Value.lname,
                userUpdateResult.Value.gender
                );

            var aiResult = await aIAuthService.UpdateUserAsync(
                            request.PatientId,
                            aiUpdateRequest,
                            Consts.FixedPassword,
                            cancellationToken
                            );

            if (aiResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
                return aiResult.Error;
            }

            await unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
            return Result.Success();
        }
        catch
        {
            // TODO: log error
            await unitOfWork.RollbackTransactionAsync(transaction, cancellationToken);
            return Error.BadRequest("Error.UpdatePatientRequest", "error occure while update patient");
        }
    }
}
