namespace Autine.Domain.Entities;

public class ThreadMessage
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid ThreadId { get; set; }
    public Guid MessageId { get; set; }
    public Message Message { get; set; } = default!;
    public virtual Patient Thread { get; set; } = default!;
}
