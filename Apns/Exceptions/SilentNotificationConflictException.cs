namespace Fitomad.Apns.Exceptions;

public class SilentNotificationConflictException : Exception
{
    private const string SilentNotificationMessage = "If you set the notification as *Silent* the fieds `alert`, `badge` and `sound` must be null";
    
    public SilentNotificationConflictException() : base(message: SilentNotificationMessage)
    {
    }
}