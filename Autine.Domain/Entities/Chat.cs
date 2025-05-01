using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Autine.Domain.Entities;

public class Chat : AuditableEntity
{
    public string UserIdOne { get; set; } = string.Empty;
    public string UserIdTwo {  get; set; } = string.Empty;
    public ICollection<Message> Messages { get; set; } = [];
}
