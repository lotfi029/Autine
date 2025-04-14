namespace Autine.Domain.Entities;

public class Bot
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Context { get; set; }
    public string Bio { get; set; }
    public string CreatorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Creator { get; set; }
    public virtual ICollection<BotPatient>? BotPatients { get; set; } = new List<BotPatient>();
}
