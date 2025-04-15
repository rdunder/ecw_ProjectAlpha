using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Service.Dtos;
using Service.Interfaces;
using System.Security.Claims;
using Ui.Asp.Mvc.Hubs;

namespace Ui.Asp.Mvc.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController(
                IHubContext<NotificationHub> hubContext,
                INotificationService notificationService) : ControllerBase
{
    private readonly IHubContext<NotificationHub> _notificationHub = hubContext;
    private readonly INotificationService _notificationService = notificationService;


    [HttpPost]
    public async Task<IActionResult> CreateNotificationAsync(NotificationDto dto)
    {
        await _notificationService.CreateNotificationAsync(dto);
        var notifications = await _notificationService.GetNotificationsAsync(Guid.NewGuid());
        var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();

        if (newNotification != null)
        {
            await _notificationHub.Clients.All.SendAsync("ReceiveNotification", newNotification);
        }
        return Ok(new {success = true});
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var userIdAsString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        if (string.IsNullOrEmpty(userIdAsString)) return Unauthorized();

        var notifications = await _notificationService.GetNotificationsAsync(Guid.Parse(userIdAsString));
        return Ok(notifications);
    }

    [HttpPost("dismiss/{id}")]
    public async Task<IActionResult> DismissNotification(Guid id)
    {
        var userIdAsString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        if (string.IsNullOrEmpty(userIdAsString)) return Unauthorized();

        await _notificationService.DismissNotificationAsync(id, Guid.Parse(userIdAsString));
        await _notificationHub.Clients.User(userIdAsString).SendAsync("NotificationDismissed", id);

        return Ok(new {success = true});
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteNotification(Guid id)
    {
        return Ok();
    }
}
