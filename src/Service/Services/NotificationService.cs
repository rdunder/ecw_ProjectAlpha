using Data.Interfaces;
using Service.Dtos;
using Service.Factories;
using Service.Interfaces;
using Service.Models;

namespace Service.Services;

public class NotificationService(
    INotificationRepository notificationRepository,
    INotificationDismissedRepository dismissedRepo,
    IUserService userService) : INotificationService
{
    private readonly INotificationRepository _repo = notificationRepository;
    private readonly INotificationDismissedRepository _dismissedRepo = dismissedRepo;
    private readonly IUserService _userService = userService;


    public async Task<bool> AddNotificationAsync(NotificationDto dto)
    {
        if (dto == null) return false;

        var entity = NotificationFactory.Create(dto);
        entity.Created = DateTime.UtcNow;

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

    //public async Task<IEnumerable<NotificationModel>> GetNotificationsAsync(Guid userId, int take = 10)
    //{

    //}
}
