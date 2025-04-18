namespace Autine.Domain.Entities;

public class ChatMessage
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid MessageId { get; set; }
    public Guid ChatId { get; set; }
    public virtual Chat Chat { get; set; } = null!;
    public Message Message { get; set; } = default!;
}