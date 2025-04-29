using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Service.Interfaces;

namespace Ui.Asp.Mvc.Hubs;

[Authorize]
public class MessageHub(IUserService userService) : Hub
{
    private readonly IUserService _userService = userService;

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string userId, string message)
    {
        if (Context.User == null) return;
        var sender = await _userService.GetByIdAsync(Guid.Parse(Context.UserIdentifier));
        await Clients.User(userId).SendAsync("RecieveMessage", $"{sender.FirstName} {sender.LastName}", sender.Title, message);
    }
}
