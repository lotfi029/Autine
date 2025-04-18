namespace Autine.Domain.Entities;

public class ThreadMessage : AuditableEntity
{
    public string Message { get; set; } = string.Empty;
    public int ThreadId { get; set; }
    public virtual Patient Thread { get; set; } = default!;
}
