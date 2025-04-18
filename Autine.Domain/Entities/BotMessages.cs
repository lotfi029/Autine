namespace Autine.Domain.Entities;

public class BotMessage : AuditableEntity
{
    public MessageStatus Status { get; set; } = MessageStatus.Sent;
    public string Content { get; set; } = string.Empty;
    public DateTime? DeliveredDate { get; set; }
    public DateTime? ReadDate { get; set; }
    public Guid BotPatientId { get; set; }
    public BotPatient BotPatient { get; set; } = default!;
}
