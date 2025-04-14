namespace Autine.Domain.Entities;

public class PatientSupervisor
{
    public string PatientId { get; set; } = string.Empty;
    public string SupervisorId { get; set; } = string.Empty;
    public bool IsSupervised { get; set; } = true;
}