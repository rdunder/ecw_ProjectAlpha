using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Ui.Asp.Mvc.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationToAll(object notification)
    {
        await Clients.All.SendAsync("AllReceiveNotification", notification);
    }

    public async Task SendNotification(object notification)
    {
        await Clients.All.SendAsync("ReceiveNotification", notification);
    }


    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        await Groups.AddToGroupAsync(Context.ConnectionId, "All");

        if (Context.User.IsInRole("Manager"))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Manager");
        }

        if (Context.User.IsInRole("Administrator"))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Administrator");
        }


        await base.OnConnectedAsync();
    }
}
