using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Notification;

namespace Fitomad.Apns;

public interface IApnsClient
{
    /// <summary>
    /// Send a notification request to the APNS service
    /// </summary>
    /// <param name="notification">The notification content</param>
    /// <param name="notificationSettings">Settings for the notification</param>
    /// <param name="deviceToken">Device unique identifier</param>
    /// <returns></returns>
    Task<ApnsResponse> SendAsync(NotificationContainer notification, NotificationSettings notificationSettings, string deviceToken); 
    /// <summary>
    /// Send a notification request to the APNS service with the default settings
    /// </summary>
    /// <param name="notification">The notification content</param>
    /// <param name="deviceToken"></param>
    /// <returns>Device unique identifier</returns>
    Task<ApnsResponse> SendAsync(NotificationContainer notification, string deviceToken);
}