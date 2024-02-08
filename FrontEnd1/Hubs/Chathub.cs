using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.ResponseCompression;
using FrontEnd1.Hubs;
using FrontEnd1.Components.Models;

namespace FrontEnd1.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    public async Task SendMessageToGroup(string groupName, string message, string user)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        //await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
    }
    public async Task AddToGroup(UserConnection model)
    {
        
        await Groups.AddToGroupAsync(Context.ConnectionId, model.ChatRoom);

        await Clients.Group(model.ChatRoom).SendAsync("ReceiveMessage", "hallo", $"{model.UserName} has joined the group {model.ChatRoom}.");

    }
    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the group {groupName}.");
    }
}