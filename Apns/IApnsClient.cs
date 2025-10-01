using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Notification;

namespace Fitomad.Apns;

public interface IApnsClient
{
    /// <summary>
    /// Send a notification request to the APNS service
    /// </summary>
    /// <param name="container">The notification content</param>
    /// <param name="notificationSettings">Settings for the notification</param>
    /// <param name="deviceToken">Device unique identifier</param>
    /// <returns></returns>
    Task<ApnsResponse> SendAsync<T>(T container, NotificationSettings notificationSettings, string deviceToken) 
        where T : NotificationContainer; 
    /// <summary>
    /// Send a notification request to the APNS service with the default settings
    /// </summary>
    /// <param name="container">The notification content</param>
    /// <param name="deviceToken"></param>
    /// <returns>Device unique identifier</returns>
    Task<ApnsResponse> SendAsync<T>(T container, string deviceToken)
        where T : NotificationContainer;
}