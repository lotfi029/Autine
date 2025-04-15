namespace Autine.Domain.Entities;

public class BotPatient
{
    public Guid BotId { get; set; }
    public string PatientId { get; set; } = default!;
    public Bot Bot { get; set; } = default!;
    //public virtual ICollection<BotMessage>? BotMessages { get; set; } = new List<BotMessage>();
}
