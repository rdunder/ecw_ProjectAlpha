using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Ui.Asp.Mvc.Hubs;

public class PresenceHub : Hub
{

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        await Clients.Others.SendAsync("UserConnected", userId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        await Clients.Others.SendAsync("UserDisconnected", userId);

        await base.OnDisconnectedAsync(exception);
    }
}
