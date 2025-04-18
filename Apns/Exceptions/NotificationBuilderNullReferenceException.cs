namespace Fitomad.Apns.Exceptions;

public class NotificationBuilderNullReferenceException : Exception
{
    private const string NotificationBuilderNullReferenceMessage = "The notification builder is null reference so we cant access from the Live Activity notification builder.";
    
    public NotificationBuilderNullReferenceException() : base(message: NotificationBuilderNullReferenceMessage)
    {
    }
}