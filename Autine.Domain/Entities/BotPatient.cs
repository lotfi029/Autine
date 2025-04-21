namespace Autine.Domain.Entities;

public class BotPatient : AuditableEntity
{
    public Guid BotId { get; set; }
    public Guid PatientId { get; set; } = default!;
    public bool IsDisabled { get; set; } = false;
    public Bot Bot { get; set; } = default!;
    public Patient Patient { get; set; } = default!;
    public virtual ICollection<BotMessage>? BotMessages { get; set; } = [];
}
