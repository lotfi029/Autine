namespace Autine.Domain.Entities;

public class BotMessage
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string Direction { get; set; }
    public DateTime SentDate { get; set; }
    public int? BotPatientId { get; set; }
    public BotPatient? BotPatient { get; set; }

}
