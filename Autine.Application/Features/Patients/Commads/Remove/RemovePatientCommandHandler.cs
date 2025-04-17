namespace Autine.Application.Features.Patients.Commads.Remove;
public class RemovePatientCommandHandler : ICommandHandler<RemovePatientCommand>
{
    public Task<Result> Handle(RemovePatientCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
