namespace Autine.Domain.Entities;

public class Patient : AuditableEntity
{
    public string PatientId { get; set; } = string.Empty;
    public bool IsSupervised { get; set; } = true;
    public string ThreadTitle { get; set; } = string.Empty;
    public ICollection<ThreadMember> Members { get; set; } = [];
    public ICollection<BotPatient> Bots { get; set; } = [];
}