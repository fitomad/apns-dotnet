using Fitomad.Apns.Entities.Notification;
using Fitomad.Apns.Exceptions;

namespace Fitomad.Apns;

public sealed class LiveActivityNotificationBuilder: ILiveActivityNotificationBuilder
{
    private readonly Notification _notification;
    private readonly WeakReference<INotificationBuilder> _notificationBuilder;

    public LiveActivityNotificationBuilder(Notification notification, INotificationBuilder notificationBuilder)
    {
        _notification = notification;
        _notificationBuilder = new WeakReference<INotificationBuilder>(notificationBuilder);
    }

    public ILiveActivityNotificationBuilder SetTimestamp(long value)
    {
        _notification.Timestamp = value;
        return this;
    }

    public ILiveActivityNotificationBuilder SetLiveActivityStaleTime(long value)
    {
        _notification.StaleDate = value;
        return this;
    }

    public ILiveActivityNotificationBuilder WithLiveActivityContent(IDictionary<string, object> content)
    {
        _notification.ContentState = content;
        return this;
    }

    public ILiveActivityNotificationBuilder SetLiveActivityEvent(LiveActivityEvent value)
    {
        _notification.Event = value.GetApnsString();
        return this;
    }

    public ILiveActivityNotificationBuilder SetLiveActivityDismissalDate(long value)
    {
        _notification.DismissalDate = value;
        return this;
    }

    public ILiveActivityNotificationBuilder SetLiveActivityAttributeType(string typeName)
    {
        _notification.AttributesType = typeName;
        return this;
    }

    public ILiveActivityNotificationBuilder SetLiveActivityStartAttributes(IDictionary<string, object> attributes)
    {
        _notification.Attributes = attributes;
        return this;
    }

    public INotificationBuilder UseRegularNotification()
    {
        INotificationBuilder weakNotificationBuilder;
        
        bool isAlive = _notificationBuilder.TryGetTarget(out weakNotificationBuilder);

        if(!isAlive)
        {
            throw new NotificationBuilderNullReferenceException();
        }

        return weakNotificationBuilder;
    }

    public Notification Build()
    {
        INotificationBuilder weakNotificationBuilder;
        
        bool isAlive = _notificationBuilder.TryGetTarget(out weakNotificationBuilder);

        if(!isAlive)
        {
            throw new NotificationBuilderNullReferenceException();
        }

        return weakNotificationBuilder.Build();
    }
}