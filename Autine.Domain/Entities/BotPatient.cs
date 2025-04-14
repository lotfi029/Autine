namespace Autine.Domain.Entities;

public class BotPatient
{
    public int Id { get; set; }
    public int BotId { get; set; }
    public Bot Bot { get; set; }
    public string PatientId { get; set; }
    public User Patient { get; set; }
    public virtual ICollection<BotMessage>? BotMessages { get; set; } = new List<BotMessage>();
}
