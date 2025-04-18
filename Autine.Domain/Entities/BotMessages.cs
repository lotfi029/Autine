namespace Autine.Domain.Entities;

public class BotMessage
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid MessageId { get; set; } 
    public Guid BotPatientId { get; set; }
    public Message Message { get; set; } = default!;
    public BotPatient BotPatient { get; set; } = default!;
}
