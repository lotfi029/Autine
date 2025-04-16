namespace Autine.Domain.Entities;

public class Patient : Entity
{
    public string PatientId { get; set; } = string.Empty;
    public string SupervisorId { get; set; } = string.Empty;
    public bool IsSupervised { get; set; } = true;
    public string ThreadTitle { get; set; } = string.Empty;
    public virtual ICollection<ThreadMember> Members { get; set; } = [];
}