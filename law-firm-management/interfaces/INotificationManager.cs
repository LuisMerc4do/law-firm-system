using law_firm_management.Dto.NotificationDto;
using law_firm_management.Models;

namespace law_firm_management.interfaces;

public interface INotificationManager
{
    Task<List<NotificationModelDto>> GetAllNotificationsAsync();
    Task<NotificationModelDto> GetNotificationByIdAsync(int notificationId);
    Task<NotificationModelDto> CreateNotificationAsync(CreateNotificationDto notificationDto);
    Task<NotificationModelDto> UpdateNotificationAsync(int notificationId, UpdateNotificationDto notificationDto);
    Task<bool> DeleteNotificationAsync(int notificationId);
}
