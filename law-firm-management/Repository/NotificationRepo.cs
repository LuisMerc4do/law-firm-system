using law_firm_management.Data;
using law_firm_management.Dto.NotificationDto;
using law_firm_management.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace law_firm_management.Repository
{
    public class NotificationRepository : INotificationManager
    {
        private readonly ApplicationDBContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<NotificationRepository> _logger;

        public NotificationRepository(ApplicationDBContext context, IMemoryCache cache, ILogger<NotificationRepository> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<NotificationModelDto>> GetAllNotificationsAsync()
        {
            if (!_cache.TryGetValue("notifications", out List<NotificationModelDto> notifications))
            {
                notifications = await _context.Notifications
                    .Select(n => new NotificationModelDto
                    {
                        NotificationId = n.NotificationId,
                        UserId = n.UserId,
                        Message = n.Message,
                        DateSent = n.DateSent
                    })
                    .ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cache.Set("notifications", notifications, cacheEntryOptions);
                _logger.LogInformation("Notifications retrieved from database and cached.");
            }
            else
            {
                _logger.LogInformation("Notifications retrieved from cache.");
            }

            return notifications;
        }

        public async Task<NotificationModelDto> GetNotificationByIdAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification == null)
            {
                _logger.LogWarning("Notification with ID {Id} not found.", notificationId);
                return null;
            }

            return new NotificationModelDto
            {
                NotificationId = notification.NotificationId,
                UserId = notification.UserId,
                Message = notification.Message,
                DateSent = notification.DateSent
            };
        }

        public async Task<NotificationModelDto> CreateNotificationAsync(CreateNotificationDto notificationDto)
        {
            var notification = new Models.NotificationModel
            {
                UserId = notificationDto.UserId,
                Message = notificationDto.Message,
                DateSent = DateTime.Now // Example: Set current date/time
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            _cache.Remove("notifications"); // Invalidate cache
            _logger.LogInformation("Notification created and cache invalidated.");

            return new NotificationModelDto
            {
                NotificationId = notification.NotificationId,
                UserId = notification.UserId,
                Message = notification.Message,
                DateSent = notification.DateSent
            };
        }

        public async Task<NotificationModelDto> UpdateNotificationAsync(int notificationId, UpdateNotificationDto notificationDto)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification == null)
            {
                _logger.LogWarning("Notification with ID {Id} not found.", notificationId);
                return null;
            }

            notification.Message = notificationDto.Message;
            notification.DateSent = DateTime.Now; // Example: Update date/time

            await _context.SaveChangesAsync();
            _cache.Remove("notifications"); // Invalidate cache
            _logger.LogInformation("Notification updated and cache invalidated.");

            return new NotificationModelDto
            {
                NotificationId = notification.NotificationId,
                UserId = notification.UserId,
                Message = notification.Message,
                DateSent = notification.DateSent
            };
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification == null)
            {
                _logger.LogWarning("Notification with ID {Id} not found.", notificationId);
                return false;
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            _cache.Remove("notifications"); // Invalidate cache
            _logger.LogInformation("Notification deleted and cache invalidated.");

            return true;
        }
    }
}
