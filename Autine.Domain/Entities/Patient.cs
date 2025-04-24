namespace Autine.Domain.Entities;

public class Patient 
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public bool IsSupervised { get; set; } = true;
    public bool IsDisabled { get; set; } = false;
    public string ThreadTitle { get; set; } = string.Empty;
    public ICollection<ThreadMember> Members { get; set; } = [];
    public ICollection<BotPatient> Bots { get; set; } = [];
}