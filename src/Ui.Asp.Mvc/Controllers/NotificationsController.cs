using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Service.Interfaces;
using Ui.Asp.Mvc.Hubs;

namespace Ui.Asp.Mvc.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController(
                IHubContext<NotificationHub> hubContext,
                INotificationService notificationService) : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;
    private readonly INotificationService _notificationService = notificationService;


}
