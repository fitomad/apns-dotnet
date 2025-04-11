using Apns.Entities.Notification;

namespace Apns;

public class SilentNotificationConflictException : Exception
{
    private const string SilentNotificationMessage = "If you set the notification as *Silent* the fieds `alert`, `badge` and `sound` must be null";
    
    public SilentNotificationConflictException() : base(message: SilentNotificationMessage)
    {
    }
}

public class NotificationBuilderNullReferenceException : Exception
{
    private const string NotificationBuilderNullReferenceMessage = "The notification builder is null reference so we cant access from the Live Activity notification builder.";
    
    public NotificationBuilderNullReferenceException() : base(message: NotificationBuilderNullReferenceMessage)
    {
    }
}

public class SoundConflictException : Exception
{
    private const string SoundMessage = "You can only set a sound name or sound settings, but not both in the same notification";
    
    public SoundConflictException() : base(message: SoundMessage)
    {
    }
}

public interface INotificationBuilder
{
    INotificationBuilder WithAlert(AlertBase alert);
    INotificationBuilder WithBadgeCount(int value);
    INotificationBuilder ClearBadgeCount();
    INotificationBuilder PlayDefaultSound();
    INotificationBuilder PlaySound(string value);
    INotificationBuilder PlaySound(SoundSettings value);
    INotificationBuilder PlaySound(string name, bool isCritical, Volume volume);
    INotificationBuilder WithThreadId(string value);
    INotificationBuilder WithCategory(string value);
    INotificationBuilder AsSilentNotification();
    INotificationBuilder EnableAppExtensionModification();
    INotificationBuilder SetTargetContentId(string value);
    INotificationBuilder SetInterruptionLevel(InterruptionLevel value);
    INotificationBuilder SetRelevanceScore(double value);
    INotificationBuilder WithFilterCriteria(string value);

    ILiveActivityNotificationBuilder UseLiveActivity();

    INotificationBuilder Reset();
    Notification Build();
}

public interface ILiveActivityNotificationBuilder
{
    ILiveActivityNotificationBuilder SetTimestamp(int value);
    ILiveActivityNotificationBuilder SetLiveActivityStaleTime(int value);
    ILiveActivityNotificationBuilder WithLiveActivityContent(IDictionary<string, object> value);
    ILiveActivityNotificationBuilder SetLiveActivityEvent(LiveActivityEvent value);
    ILiveActivityNotificationBuilder SetLiveActivityDismissalDate(DateTime value);
    ILiveActivityNotificationBuilder SetLiveActivityDismissalDate(int timestamp);
    ILiveActivityNotificationBuilder SetLiveActivityAttributeType(string typeName);
    ILiveActivityNotificationBuilder SetLiveActivityStartAttributes(IDictionary<string, object> attributes);
    
    INotificationBuilder UseRegularNotification();
    
    Notification Build();
}

public sealed class NotificationBuilder: INotificationBuilder
{
    private Notification _notification;

    public NotificationBuilder()
    {
        _notification = new Notification();
    }

    public INotificationBuilder WithAlert(AlertBase alert)
    {
        _notification.Alert = alert;
        return this;
    }

    public INotificationBuilder WithBadgeCount(int value)
    {
        _notification.Badge = value;
        return this;
    }

    public INotificationBuilder ClearBadgeCount()
    {
        _notification.Badge = 0;
        return this;
    }
    
    public INotificationBuilder PlayDefaultSound()
    {
        return PlaySound("default");
    }
    
    public INotificationBuilder PlaySound(string value)
    {
        _notification.SoundName = value;
        return this;
    }

    public INotificationBuilder PlaySound(string name, bool isCritical, Volume volume)
    {
        var soundSettings = new SoundSettings()
        {
            Name = name,
            Critial = isCritical ? 1 : 0,
            Volume = volume.GetApnsValue()
        };

        return PlaySound(soundSettings);
    }
    
    public INotificationBuilder PlaySound(SoundSettings value)
    {
        _notification.Sound = value;
        return this;
    }

    public INotificationBuilder WithThreadId(string value)
    {
        _notification.ThreadId = value;
        return this;
    }

    public INotificationBuilder WithCategory(string value)
    {
        _notification.Category = value;
        return this;
    }

    public INotificationBuilder AsSilentNotification()
    {
        _notification.ContentAvailable = 1;
        return this;
    }

    public INotificationBuilder EnableAppExtensionModification()
    {
        _notification.MutableContent = 1;
        return this;
    }

    public INotificationBuilder SetTargetContentId(string value)
    {
        _notification.TargetContentId = value;
        return this;
    }

    public INotificationBuilder SetInterruptionLevel(InterruptionLevel level)
    {
        _notification.InterruptionLevel = level.GetApnsValue();
        return this;
    }

    public INotificationBuilder SetRelevanceScore(double value)
    {
        _notification.RelevanceScore = value;
        return this;
    }

    public INotificationBuilder WithFilterCriteria(string value)
    {
        _notification.FilterCriteria = value;
        return this;
    }

    public ILiveActivityNotificationBuilder UseLiveActivity()
    {
        var liveActivityBuilder = new LiveActivityNotificationBuilder(_notification, this);
        return liveActivityBuilder;
    }

    public INotificationBuilder Reset()
    {
        _notification = new Notification();
        return this;
    }

    public Notification Build()
    {
        if(_notification.ContentAvailable == 1)
        {
            if(_notification.Alert != null || _notification.Badge != null || _notification.Sound != null)
            {
                throw new SilentNotificationConflictException();
            }
        }

        if(_notification.SoundName != null && _notification.Sound != null)
        {
            throw new SoundConflictException();
        }
        
        return _notification;
    }
}

public sealed class LiveActivityNotificationBuilder: ILiveActivityNotificationBuilder
{
    private Notification _notification;
    private WeakReference<INotificationBuilder> _notificationBuilder;

    public LiveActivityNotificationBuilder(Notification notification, INotificationBuilder notificationBuilder)
    {
        _notification = notification;
        _notificationBuilder = new WeakReference<INotificationBuilder>(notificationBuilder);
    }

    public ILiveActivityNotificationBuilder SetTimestamp(int value)
    {
        _notification.Timestamp = value;
        return this;
    }

    public ILiveActivityNotificationBuilder SetLiveActivityStaleTime(int value)
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
        _notification.Event = value.GetApnsValue();
        return this;
    }

    public ILiveActivityNotificationBuilder SetLiveActivityDismissalDate(DateTime value)
    {
        return this;
    }

    public ILiveActivityNotificationBuilder SetLiveActivityDismissalDate(int timestamp)
    {
        _notification.DismissalDate = timestamp;
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