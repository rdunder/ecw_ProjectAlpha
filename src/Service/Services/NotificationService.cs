using Data.Entities;
using Data.Enums;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;

namespace Service.Services;
public class NotificationService(
    INotificationRepository notificationRepository,
    INotificationDismissedRepository dismissedRepo,
    UserManager<UserEntity> userManager) : INotificationService
{
    private readonly INotificationRepository _repo = notificationRepository;
    private readonly INotificationDismissedRepository _dismissedRepo = dismissedRepo;
    private readonly UserManager<UserEntity> _userManager = userManager;


    public async Task<bool> CreateNotificationAsync(NotificationDto dto)
    {
        if (dto == null) return false;

        var entity = NotificationFactory.Create(dto);

        await _repo.BeginTransactionAsync();
        try
        {
            await _repo.CreateAsync(entity);
            await _repo.SaveChangesAsync();
            await _repo.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            await _repo.RollbackTransactionAsync();
            return false;
        }
    }

    public async Task<IEnumerable<NotificationModel>> GetNotificationsAsync(Guid userId, int take = 10)
    {
        var user = _userManager.Users.Where(x => x.Id == userId).FirstOrDefault();
        var userRoles = await _userManager.GetRolesAsync(user);

        if (userRoles.Contains("Viewer")) return new List<NotificationModel>();

        var targetGroups = new List<NotificationTargetGroup>()
        {
            NotificationTargetGroup.All
        };

        switch (userRoles.FirstOrDefault())
        {
            case "User":
                targetGroups.Add(NotificationTargetGroup.Users);
                break;

            case "Manager":
                targetGroups.Add(NotificationTargetGroup.Managers);
                break;

            case "Administrator":
                targetGroups.Add(NotificationTargetGroup.Admins);
                break;
        }

        var dismissedIds = await _dismissedRepo.GetDismissedNotificationIdsAsync(userId);

        var notifications = await _repo.GetAllAsync(notification =>
            notification
                .Where(n => !dismissedIds.Contains(n.Id) && targetGroups.Contains(n.TargetGroup))
                .OrderByDescending(n => n.Created)
                .Take(take));

        return notifications.Select(n => NotificationFactory.Create(n));
    }

    public async Task DismissNotificationAsync(Guid notificationId, Guid userId)
    {
        if (await _dismissedRepo.AlreadyExistsAsync(x => x.NotificationId == notificationId && x.UserId == userId))
            return;

        var entity = new NotificationDismissedEntity
        {
            UserId = userId,
            NotificationId = notificationId,
        };

        await _dismissedRepo.BeginTransactionAsync();
        try
        {
            await _dismissedRepo.CreateAsync(entity);
            await _dismissedRepo.SaveChangesAsync();
            await _dismissedRepo.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _dismissedRepo.RollbackTransactionAsync();
        }
    }

    public async Task<IEnumerable<NotificationModel>> GetAllAsync()
    {
        var notifications = await _repo.GetAllAsync();
        return notifications.Select(n => NotificationFactory.Create(n));
    }
}
