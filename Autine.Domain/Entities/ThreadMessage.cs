namespace Autine.Domain.Entities;

public class ThreadMessage
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid ThreadMemberId { get; set; }
    public Guid MessageId { get; set; }
    public Message Message { get; set; } = default!;
    public ThreadMember ThreadMember { get; set; } = default!;
}