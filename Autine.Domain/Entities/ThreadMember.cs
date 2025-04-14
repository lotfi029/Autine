using System.Text.Json.Serialization;

namespace Autine.Domain.Entities;

public class ThreadMember
{
    public int Id { get; set; }
    //fk to userapp
    public string UserId { get; set; }  
    //fk to thread
    public int ThreadId { get; set; }
    [JsonIgnore]
    public virtual ChatThread Thread { get; set; } = null!;
    public virtual User User { get; set; } = null!;

}