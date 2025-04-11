namespace Apns.Entities.Notification;

public enum Priority
{
    /// <summary>
    /// Send the notification immediately. Default value
    /// </summary>
    High,
    /// <summary>
    /// Send the notification based on power considerations on the user’s device
    /// </summary>
    Medium,
    /// <summary>
    /// Prioritize the device’s power considerations over all other factors for delivery, and prevent awakening the device
    /// </summary>
    Low
}

public static class PriorityExtensions
{
    public static string GetApnsValue(this Priority priority)
    {
        var value = priority switch
        {
            Priority.High => "10",
            Priority.Medium => "5",
            Priority.Low => "1",
            _ => throw new ArgumentOutOfRangeException(nameof(priority), priority, null)
        };

        return value;
    }
}