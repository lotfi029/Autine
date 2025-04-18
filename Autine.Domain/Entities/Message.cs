namespace Autine.Domain.Entities;

public class Message
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public MessageStatus Status { get; set; } = MessageStatus.Sent;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public string SenderId { get; set; } = string.Empty;

    public ICollection<ThreadMessage>? ThreadMessages { get; set; }
    public ICollection<BotMessage>? BotMessages { get; set; }
}
