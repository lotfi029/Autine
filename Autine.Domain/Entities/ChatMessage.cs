using System.Text.Json.Serialization;

namespace Autine.Domain.Entities;

public class ChatMessage
{
    //pk
    public int Id { get; set; }

    public string Message { get; set; } = string.Empty;

    //[JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime SentDate { get; set; }= DateTime.Now;
    //[JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime? DeliveredDate { get; set; }

    //[JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime? SeenDate { get; set; }
    public string Direction { get; set; }= string.Empty;
    //fk
    public int ChatId { get; set; }

    //NP
    public virtual Chat Chat { get; set; } = null!;

}

