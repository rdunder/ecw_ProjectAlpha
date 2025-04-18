using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Ui.Asp.Mvc.Hubs;

public class PresenceHub(ILogger<PresenceHub> logger) : Hub
{
    private readonly ILogger<PresenceHub> _logger = logger;
    private static readonly Dictionary<string, string> ConnectedUsers = [];

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("SignalR PresenceHUB failed because of empty ID");
            return;
        }

        ConnectedUsers[userId] = Context.ConnectionId;

        await Clients.Others.SendAsync("UserConnected", userId);

        await Clients.Caller.SendAsync("OnlineUsers", ConnectedUsers.Keys);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        ConnectedUsers.Remove(userId);

        await Clients.Others.SendAsync("UserDisconnected", userId);

        await base.OnDisconnectedAsync(exception);
    }
}
