namespace Autine.Domain.Entities;

public class ThreadMember : Entity
{
    public string UserId { get; set; } = string.Empty;
    public Guid ThreadId { get; set; }
    public virtual Patient Thread { get; set; } = null!;
}