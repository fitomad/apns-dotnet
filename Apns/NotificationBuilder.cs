using Fitomad.Apns.Entities.Notification;
using Fitomad.Apns.Exceptions;
using Fitomad.Apns.Services.Validation;

namespace Fitomad.Apns;

public sealed class NotificationBuilder: INotificationBuilder
{
    private Notification _notification = new();

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
        _notification.Sound = value;
        return this;
    }

    public INotificationBuilder PlaySound(string name, bool isCritical, Volume volume)
    {
        var soundSettings = new SoundSettings
        {
            Name = name,
            Critial = isCritical ? 1 : 0,
            Volume = volume.Decibels
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
        _notification.InterruptionLevel = level.GetApnsString();
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
        new Rule()
            .Where(() => _notification.ContentAvailable == 1)
            .VerifyThat(() => _notification.Alert == null)
            .VerifyThat(() => _notification.Badge == null)
            .VerifyThat(() => _notification.Sound == null)
            .OnFailure(() => throw new SilentNotificationConflictException())
            .Validate();

        return _notification;
    }
}
