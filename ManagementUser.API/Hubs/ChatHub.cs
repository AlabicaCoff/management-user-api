using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using ManagementUser.API.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace ManagementUser.API.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> MapUserNameToGroup = new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, string> MapConnectionIdToUserName = new ConcurrentDictionary<string, string>();

        public async Task JoinChatRoom(string roomName, string userName)
        {
            if (string.IsNullOrWhiteSpace(roomName) || string.IsNullOrWhiteSpace(userName))
            {
                await Clients.Caller.SendAsync("ReceiveError", "Room name and user name cannot be empty.");
                return;
            }

            var connectionId = Context.ConnectionId;

            // Store user information
            MapConnectionIdToUserName[connectionId] = userName;
            
            // Add user to SingalR group
            await Groups.AddToGroupAsync(connectionId, roomName);
        
            // Notify the group that new user joined
            await Clients.Group(roomName).SendAsync("UserJoined", userName);

            // Send confirmation to the user
            await Clients.Caller.SendAsync("JoinedRoom", userName);
        }

        public async Task SendMessage(string roomName, string message)
        {
            if (string.IsNullOrWhiteSpace(roomName) || string.IsNullOrWhiteSpace(message))
            {
                await Clients.Caller.SendAsync("ReceiveError", "Room name and message cannot be empty.");
                return;
            }

            var connectionId = Context.ConnectionId;

            if (!MapConnectionIdToUserName.TryGetValue(connectionId, out var userName))
            {
                await Clients.Caller.SendAsync("ReceiveError", "User information not found.");
                return;
            }

            var messageData = new MessageDto
            {
                UserName = userName,
                Message = message,
                Timestamp = DateTime.UtcNow
            };
            
            await Clients.Group(roomName).SendAsync("ReceiveMessage", messageData);
        }

        public async Task LeaveChatRoom(string roomName)
        {
            var connectionId = Context.ConnectionId;

            if (MapConnectionIdToUserName.TryRemove(connectionId, out var userName))
            {
                // Remove user from SingalR group
                await Groups.RemoveFromGroupAsync(connectionId, roomName);
                
                // Notify the group that user left
                await Clients.Group(roomName).SendAsync("UserLeft", userName);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;

            if (MapConnectionIdToUserName.TryRemove(connectionId, out var userName))
            {
                // Notify the group that user disconnected
                await Clients.All.SendAsync("UserDisconnected", userName);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}