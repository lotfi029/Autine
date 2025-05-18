using Autine.Application.Contracts.Chats;
using Autine.Application.Contracts.UserBots;
using Autine.Application.Features.Messages.Commands;
using Autine.Application.Features.Messages.Queries.GetChat;
using Autine.Application.Features.Messages.Queries.GetConnections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Hybrid;
using System.Collections.Concurrent;

namespace Autine.Api.Hubs;

[Authorize(Roles = $"{DefaultRoles.Parent.Name}, {DefaultRoles.Doctor.Name}")]
public class ChatHub(
    HybridCache _cache,
    ISender _sender) : Hub<IChatHubClient>
{
    private const string _key = "online-users";
    private async Task<IEnumerable<UserChatResponse>> GetContactsAsync(string userId)
    {
        var command = new GetUserConnectionsQuery(userId);
        var result = await _sender.Send(command);

        if (result.IsFailure)
            return [];

        return result.Value;
    }
    private async Task<ConcurrentDictionary<string, HashSet<string>>> LoadCurrentAsync()
    {
        var data = await _cache.GetOrCreateAsync(
            _key,
            factory: async (ct) => await Task.FromResult(new ConcurrentDictionary<string, HashSet<string>>())
            );

        return data;
    }
        
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier!;
        var connId = Context.ConnectionId!;
        var contacts = await GetContactsAsync(userId);
        var online = await LoadCurrentAsync();

        online.AddOrUpdate(
            userId,
            _ => [connId],
            (_, set) =>
            {
                lock (set)
                {
                    set.Add(connId);
                }
                return set;
            });

        await _cache.SetAsync(_key, online);

        var myOnlineConnections = online.Keys.Intersect(contacts.Select(e => e.Id));
        var myOnlineUsers = contacts.Where(e => myOnlineConnections.Contains(e.Id));
        await Clients.Users(myOnlineConnections).OnlineUser(myOnlineUsers);
        await Clients.Caller.OnlineUser(myOnlineUsers);


        await base.OnConnectedAsync();
    }
    public async Task SendMessage(UserMessageRequest request)
    {
        if (string.IsNullOrEmpty(request.ReceiverId) || string.IsNullOrEmpty(request.Content))
            return;
        
        var userId = Context.UserIdentifier!;

        var command = new SendDMCommand(userId, request.ReceiverId, request.Content);
        var result = await _sender.Send(command);


        if (result.IsSuccess)
        {
            var messageRespnonse = new MessageResponse(
                result.Value.Id,
                result.Value.Content,
                result.Value.Timestamp,
                result.Value.Status,
                false
                );
            await Clients.User(request.ReceiverId).ReceiveMessage(messageRespnonse);
            await Clients.Caller.ReceiveMessage(result.Value);
        }

    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier!;
        var connId = Context.ConnectionId;

        var contacts = await GetContactsAsync(userId);

        var online = await LoadCurrentAsync();

        if (online.TryGetValue(userId, out var set))
        {
            lock(set)
            {
                set.Remove(connId);
            }

            if (set.Count == 0)
                online.TryRemove(userId, out _);
        }
        await _cache.SetAsync(_key, online);

        var myOnlineConnections = online.Keys.Intersect(contacts.Select(e => e.Id));
        var myOnlineUsers = contacts.Where(e => myOnlineConnections.Contains(e.Id));
        if (!contacts.Any())
        {
            await Clients.Users(myOnlineConnections).OnlineUser(myOnlineUsers);
        }


        await base.OnDisconnectedAsync(exception);
    }
    public async Task GetChatHistory(string contactId)
    {
        
        var userId = Context.UserIdentifier!;
        var query = new GetChatByIdQuery(userId, contactId);
        var result = await _sender.Send(query);

        if (result.IsSuccess)
        {
            await Clients.Caller.ReceiveChatHistory(result.Value);
        }

    }
}

public interface IChatHubClient
{
    Task OnlineUser(IEnumerable<UserChatResponse> users);
    Task ReceiveMessage(MessageResponse response);
    Task ReceiveChatHistory(DetailedChatResponse chat);
}