namespace Autine.Domain.Entities;

public class BotPatient
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid BotId { get; set; }
    public Guid? PatientId { get; set; } = default!;
    public string? UserId { get; set; } = default!;
    public bool IsUser { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Bot Bot { get; set; } = default!;
    public Patient? Patient { get; set; } = default!;
    public virtual ICollection<BotMessage>? BotMessages { get; set; } = [];
}