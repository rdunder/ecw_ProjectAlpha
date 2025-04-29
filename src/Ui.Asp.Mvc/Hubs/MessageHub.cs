using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Ui.Asp.Mvc.Hubs;

[Authorize]
public class MessageHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string userId, string message)
    {
        var sender = Context.User.Identity.Name;
        if (sender == null) return;

        await Clients.User(userId).SendAsync("RecieveMessage", sender, message);
    }
}
