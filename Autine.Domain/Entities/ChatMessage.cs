namespace Autine.Domain.Entities;

public class ChatMessage : AuditableEntity
{
    public string Message { get; set; } = string.Empty;
    public DateTime? DeliveredDate { get; set; }
    public DateTime? SeenDate { get; set; }
    public string Direction { get; set; }= string.Empty;
    public int ChatId { get; set; }
    public virtual Chat Chat { get; set; } = null!;

}

