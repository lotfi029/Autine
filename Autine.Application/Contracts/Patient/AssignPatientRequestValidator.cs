namespace Autine.Application.Contracts.Patient;

public class AssignPatientRequestValidator : AbstractValidator<AssignPatientRequest>
{
    public AssignPatientRequestValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.AssigneeId).NotEmpty();
    }
}