using Autine.Application.Contracts.Profiles;

namespace Autine.Application.Features.Profiles.Commands;
public record UpdateProfileCommand(string UserId, UpdateProfileRequest UpdateRequest) : ICommand;
