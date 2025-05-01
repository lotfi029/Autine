using Autine.Application.Contracts.UserBots;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Autine.Api.Hubs
{
    // User connection tracking dictionary - maps user IDs to their connection IDs
    public static class UserConnectionManager
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>> _userConnections = new();

        public static void AddConnection(string userId, string connectionId)
        {
            _userConnections.AddOrUpdate(
                userId,
                new HashSet<string> { connectionId },
                (_, connections) =>
                {
                    connections.Add(connectionId);
                    return connections;
                });
        }

        public static void RemoveConnection(string userId, string connectionId)
        {
            if (_userConnections.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);

                if (connections.Count == 0)
                {
                    _userConnections.TryRemove(userId, out _);
                }
            }
        }

        public static HashSet<string> GetConnections(string userId)
        {
            return _userConnections.TryGetValue(userId, out var connections)
                ? connections
                : new HashSet<string>();
        }

        public static bool IsUserOnline(string userId)
        {
            return _userConnections.ContainsKey(userId) &&
                   _userConnections[userId].Count > 0;
        }

        public static IEnumerable<string> GetOnlineUsers()
        {
            return _userConnections.Keys.ToList();
        }
    }

    // Message model
    public class ChatMessage
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }

    [Authorize]
    public class ChatHub(ISender sender) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            string userId = Context.User!.GetUserId()!;


            UserConnectionManager.AddConnection(userId, Context.ConnectionId);

            await Clients.All.SendAsync("UserConnected", userId);

            await Clients.Caller.SendAsync("OnlineUsers", UserConnectionManager.GetOnlineUsers());

            await base.OnConnectedAsync();
        }

        // Called when a user disconnects from the hub
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.User!.GetUserId()!;

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
        public async Task SendDirectMessage(UserMessageRequest request)
        {
            string senderId = Context.User!.GetUserId()!;

            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("Sender ID is required");
            }

            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = request.ReciverId,
                Content = request.Content,
                Timestamp = DateTime.UtcNow
            };

            // Get all the receiver's connections
            var connections = UserConnectionManager.GetConnections(receiverId);

            // Send the message to all the receiver's active connections
            if (connections.Any())
            {
                foreach (var connectionId in connections)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                }
            }

            // Also send back to sender for confirmation
            await Clients.Caller.SendAsync("MessageSent", message);
        }

        // Check if a specific user is online
        public bool IsUserOnline(string userId)
        {
            return UserConnectionManager.IsUserOnline(userId);
        }

        // Get all currently online users
        public IEnumerable<string> GetOnlineUsers()
        {
            return UserConnectionManager.GetOnlineUsers();
        }
    }
}