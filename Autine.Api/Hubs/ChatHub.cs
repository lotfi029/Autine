using Autine.Application.Contracts.UserBots;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Autine.Api.Hubs;

// User connection tracking dictionary - maps user IDs to their connection IDs and user info
public static class UserConnectionManager
{
    private static readonly ConcurrentDictionary<string, UserInfo> _userConnections = new();

    public class UserInfo
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public HashSet<string> ConnectionIds { get; set; } = new HashSet<string>();
        public DateTime LastSeen { get; set; }
    }

    public static void AddConnection(string userId, string username, string connectionId)
    {
        _userConnections.AddOrUpdate(
            userId,
            new UserInfo
            {
                UserId = userId,
                Username = username,
                ConnectionIds = new HashSet<string> { connectionId },
                LastSeen = DateTime.UtcNow
            },
            (_, info) =>
            {
                info.ConnectionIds.Add(connectionId);
                info.LastSeen = DateTime.UtcNow;
                // Update username if provided
                if (!string.IsNullOrEmpty(username))
                {
                    info.Username = username;
                }
                return info;
            });
    }

    public static void RemoveConnection(string userId, string connectionId)
    {
        if (_userConnections.TryGetValue(userId, out var info))
        {
            info.ConnectionIds.Remove(connectionId);
            info.LastSeen = DateTime.UtcNow;

            if (info.ConnectionIds.Count == 0)
            {
                // Keep the user in the dictionary but with empty connections
                // This allows us to track their last seen time
            }
        }
    }

    public static HashSet<string> GetConnections(string userId)
    {
        return _userConnections.TryGetValue(userId, out var info)
            ? info.ConnectionIds
            : new HashSet<string>();
    }

    public static bool IsUserOnline(string userId)
    {
        return _userConnections.TryGetValue(userId, out var info) &&
               info.ConnectionIds.Count > 0;
    }

    public static IEnumerable<UserInfo> GetOnlineUsers()
    {
        var result = new List<UserInfo>();
        foreach (var item in _userConnections)
        {
            if (item.Value.ConnectionIds.Count > 0)
            {
                result.Add(item.Value);
            }
        }
        return result;
    }

    public static string GetUsername(string userId)
    {
        return _userConnections.TryGetValue(userId, out var info)
            ? info.Username
            : userId;
    }
}

// Message model
public class ChatMessage
{
    public string Id { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; }
}

[Authorize]
public class DMChatHub(ISender _sender) : Hub<IDMClient>
{
    private static readonly Dictionary<string, HashSet<string>> _userConnections = [];
    private static readonly object _lock = new();
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier!;
        var connectionId = Context.ConnectionId!;

        AddConnectedUser(userId, connectionId);

        await Clients.All.ConnectedUser($"this user is connecting {userId}");

        await Clients.All.OnlineUsers(GetOnlineUser());

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string user = Context.UserIdentifier!;
        string connectionId = Context.ConnectionId;

        await Clients.All.OnDiconnectedUser($"this user is diconnecting {user}");
        

        await base.OnDisconnectedAsync(exception);
    }

    //public async Task SendMessage(UserMessageRequest userMessageRequest, CancellationToken ct = default)
    //{
    //    await Clients.Client(userMessageRequest.ReciverId).RecieveMessage()
    //}

    private static void AddConnectedUser(string userId, string connectionId)
    {
        lock (_lock)
        {
            if (!_userConnections.TryGetValue(userId, out var value))
            {
                value = [];
                _userConnections[userId] = value;
            }

            value.Add(connectionId);
        }
    }
    private static void RemoveUserConnection(string userId, string connectionId)
    {
        lock (_lock)
        {
            if (_userConnections.TryGetValue(userId, out var value))
            {
                value.Remove(connectionId);

                if (value.Count == 0)
                    _userConnections.Remove(userId);
            }
        }
    }
    private static HashSet<string> GetConnections(string userId)
    {
        lock(_lock)
        {
            return _userConnections.TryGetValue(userId, out var value)
                ? value
                : [];
        }
    }
    private static IEnumerable<string> GetOnlineUser()
    {
        lock(_lock) 
        {
            return _userConnections.Keys;
        }
    } 

}
public interface IDMClient
{
    Task RecieveMessage(MessageResponse request);
    Task ConnectedUser(string userId);
    Task OnDiconnectedUser(string userId);
    Task OnlineUsers(IEnumerable<string> onelineUsers);
    Task MessageRead();
}



public interface IChatCacheService
{
    Task AddUserOnlineAsync(string userId);
    Task RemoveUserOnlineAsync(string userId);
    Task<IEnumerable<string>> GetOnlineUsersAsync();
}

public class ChatCacheService(HybridCache _cache) : IChatCacheService
{
    private const string OnlineKey = "online-users";
    private static readonly SemaphoreSlim _mutex = new SemaphoreSlim(1, 1);

    private readonly IHybridCache _cache;
    private readonly HybridCacheEntryOptions _hybridOptions;

    public ChatCacheService(IHybridCache cache)
    {
        _cache = cache;

        // configure your memory + distributed expirations here
        _hybridOptions = new HybridCacheEntryOptions
        {
            MemoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            },
            DistributedCacheEntryOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            }
        };
    }

    public async Task AddUserOnlineAsync(string userId)
    {
        await _mutex.WaitAsync();
        try
        {
            var set = await GetCurrentSetAsync();
            set.Add(userId);
            await SetCurrentSetAsync(set);
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task RemoveUserOnlineAsync(string userId)
    {
        await _mutex.WaitAsync();
        try
        {
            var set = await GetCurrentSetAsync();
            set.Remove(userId);
            await SetCurrentSetAsync(set);
        }
        finally
        {
            _mutex.Release();
        }
    }

    public Task<IEnumerable<string>> GetOnlineUsersAsync()
        => GetCurrentSetAsync();

    private async Task<HashSet<string>> GetCurrentSetAsync()
    {
        // Try to fetch the raw bytes; if missing, we'll treat as empty
        var data = await _cache.GetAsync<byte[]>(
            OnlineKey,
            factory: () => Task.FromResult<byte[]>(Array.Empty<byte>()),
            options: _hybridOptions
        );

        if (data == null || data.Length == 0)
            return new HashSet<string>();

        return JsonSerializer.Deserialize<HashSet<string>>(data)!
               ?? new HashSet<string>();
    }

    private Task SetCurrentSetAsync(HashSet<string> set)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(set);
        return _cache.SetAsync(
            OnlineKey,
            bytes,
            options: _hybridOptions
        );
    }
}

// Main SignalR Hub
public class ChatHub : Hub
{
    // Store messages in memory (in a real app, use a database)
    private static readonly ConcurrentDictionary<string, bool> _messageReadStatus = new();

    // Called when a user connects to the hub
    public override async Task OnConnectedAsync()
    {
        string userId = Context.GetHttpContext().Request.Query["userId"];
        string username = Context.GetHttpContext().Request.Query["username"];

        if (string.IsNullOrEmpty(userId))
        {
            throw new HubException("User ID is required");
        }

        UserConnectionManager.AddConnection(userId, username, Context.ConnectionId);

        // Notify everyone that this user is online
        await Clients.All.SendAsync("UserConnected", userId, username);

        // Send the current online users list to the connecting client
        await Clients.Caller.SendAsync("OnlineUsers", UserConnectionManager.GetOnlineUsers());
        
        await base.OnConnectedAsync();
    }

    // Called when a user disconnects from the hub
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        string userId = Context.GetHttpContext().Request.Query["userId"];

        if (!string.IsNullOrEmpty(userId))
        {
            UserConnectionManager.RemoveConnection(userId, Context.ConnectionId);

            // If user has no more active connections, notify everyone they went offline
            if (!UserConnectionManager.IsUserOnline(userId))
            {
                await Clients.All.SendAsync("UserDisconnected", userId);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    // Send a direct message to a specific user
    public async Task SendDirectMessage(string receiverId, string content, string messageId)
    {
        string senderId = Context.GetHttpContext().Request.Query["userId"];

        if (string.IsNullOrEmpty(senderId))
        {
            throw new HubException("Sender ID is required");
        }

        // Create message object
        var message = new ChatMessage
        {
            Id = messageId,
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };

        // Get all the receiver's connections
        var connections = UserConnectionManager.GetConnections(receiverId);

        // Send the message to all the receiver's active connections
        if (connections.Count != 0)
        {
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
        }

        // Also send back to sender for confirmation
        await Clients.Caller.SendAsync("MessageSent", message);
    }

    // Mark a message as read
    public async Task MarkMessageAsRead(string messageId, string senderId)
    {
        string readerId = Context.GetHttpContext().Request.Query["userId"];

        if (string.IsNullOrEmpty(readerId))
        {
            throw new HubException("Reader ID is required");
        }

        // Mark the message as read
        _messageReadStatus[messageId] = true;

        // Notify the sender that their message has been read
        var senderConnections = UserConnectionManager.GetConnections(senderId);
        foreach (var connectionId in senderConnections)
        {
            await Clients.Client(connectionId).SendAsync("MessageRead", messageId, readerId);
        }
    }

    // Check if a specific user is online
    public bool IsUserOnline(string userId)
    {
        return UserConnectionManager.IsUserOnline(userId);
    }

    // Get all currently online users
    public IEnumerable<UserConnectionManager.UserInfo> GetOnlineUsers()
    {
        return UserConnectionManager.GetOnlineUsers();
    }
}