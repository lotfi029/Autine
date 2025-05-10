namespace Autine.Domain.Entities;

public class Message
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public MessageStatus Status { get; set; } = MessageStatus.Sent;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
    public string? SenderId { get; set; }

    public Guid? ChatId { get; set; }
    public Chat? Chat { get; set; }

    public Guid? BotId { get; set; }
    public BotMessage? Bot { get; set; }
}


// botuser - thread - dm