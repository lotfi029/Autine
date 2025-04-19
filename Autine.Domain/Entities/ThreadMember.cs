namespace Autine.Domain.Entities;

public class ThreadMember : AuditableEntity
{
    public string MemberId { get; set; } = string.Empty;
    public Guid PatientId { get; set; }
    public virtual Patient Patient { get; set; } = default!;
    public virtual ICollection<ThreadMessage>? Messages { get; set; }
}