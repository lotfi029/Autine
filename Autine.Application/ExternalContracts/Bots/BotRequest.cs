namespace Autine.Application.ExternalContracts.Bots;
public record BotRequest(
    string model_name,
    string model_context,
    string model_bio
    );