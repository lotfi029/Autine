namespace Autine.Domain.Entities;

public class Chat
{
    public int Id { get; set; }

    //fk to user
    public string UserId { get; set; } = string.Empty;
    public User ApplicationUser { get; set; } = null!;

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

}
