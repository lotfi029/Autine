namespace Autine.Domain.Entities;

public class Bot
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public string Context { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string CreatorId { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //public virtual ICollection<BotPatient>? BotPatients { get; set; } = new List<BotPatient>();
}
