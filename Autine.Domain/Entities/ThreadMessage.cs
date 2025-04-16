namespace Autine.Domain.Entities;

public class ThreadMessage
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime SentDate { get; set; } = DateTime.UtcNow;
    public int ThreadId { get; set; }  
    public string SenderId { get; set; } = string.Empty;
    //public virtual ChatThread Thread { get; set; } = null!;
    public virtual User Sender { get; set; } = null!;
}
