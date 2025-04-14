
using System.Text.Json.Serialization;

namespace Autine.Domain.Entities;
public class ChatThread
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string OwnerId { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public bool IsActive { get; set; }=true;
    public virtual User Doctor { get; set; } = null!;
    public virtual User Patient { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<ThreadMember> Members { get; set; } = new List<ThreadMember>();
    public virtual ICollection<ThreadMessage> Messages { get; set; } = new List<ThreadMessage>();
}
