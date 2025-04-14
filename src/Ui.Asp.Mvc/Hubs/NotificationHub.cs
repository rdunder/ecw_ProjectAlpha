﻿using Microsoft.AspNetCore.SignalR;

namespace Ui.Asp.Mvc.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationToAll(object notification)
    {
        await Clients.All.SendAsync("AllReceiveNotification", notification);
    }

    public async Task SendNotificationToUsers(object notification)
    {
        await Clients.All.SendAsync("UserReceiveNotification", notification);
    }

    public async Task SendNotificationToManagers(object notification)
    {
        await Clients.All.SendAsync("ManagerReceiveNotification", notification);
    }

    public async Task SendNotificationToAdmins(object notification)
    {
        await Clients.All.SendAsync("AdminReceiveNotification", notification);
    }
}
