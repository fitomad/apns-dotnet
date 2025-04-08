namespace Apns.Entities.Notification;

public enum Priority
{
    /// <summary>
    /// Send the notification immediately. Default value
    /// </summary>
    High = 10,
    /// <summary>
    /// Send the notification based on power considerations on the user’s device
    /// </summary>
    Medium = 5,
    /// <summary>
    /// Prioritize the device’s power considerations over all other factors for delivery, and prevent awakening the device
    /// </summary>
    Low = 1,
}