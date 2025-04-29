using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Ui.Asp.Mvc.Hubs;

[Authorize]
public class MessageHub(UserManager<UserEntity> userManager) : Hub
{
    private readonly UserManager<UserEntity> _userManager = userManager;

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string userId, string message)
    {
        if (Context.User == null) return;
        var sender = await _userManager.GetUserAsync(Context.User);
        //  chanmge sender to send first last name and role

        await Clients.User(userId).SendAsync("RecieveMessage", sender, message);
    }
}
