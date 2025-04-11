using Apns.Entities.Notification;

namespace Apns.Test;

public class NotificationBuilderTests
{
    [Fact]
    public void TestSilentNotificationErrorAlert()
    {
        Assert.Throws<SilentNotificationConflictException>(() =>
        {
            var simpleAlert = new Alert()
            {
                Title = "Title",
                Body = "Body"
            };
            
            new NotificationBuilder()
                .WithAlert(simpleAlert)
                .AsSilentNotification()
                .Build();
        });
    }
    
    [Fact]
    public void TestSilentNotificationBadgeAlert()
    {
        Assert.Throws<SilentNotificationConflictException>(() =>
        {
            new NotificationBuilder()
                .WithBadgeCount(1)
                .AsSilentNotification()
                .Build();
        });
    }
    
    [Fact]
    public void TestSilentNotificationSound()
    {
        Assert.Throws<SilentNotificationConflictException>(() =>
        {
            var sampleSound = new SoundSettings()
            {
                Critial = 1,
                Name = "custom-sound",
                Volume = Volume.High.GetApnsValue()
            };
            
            new NotificationBuilder()
                .PlaySound(sampleSound)
                .AsSilentNotification()
                .Build();
        });
    }
    
    [Fact]
    public void TestSilentNotification()
    {
        Notification notification = new NotificationBuilder()
                .AsSilentNotification()
                .Build();
        
        Assert.Equal(notification.ContentAvailable, 1);
    }
    
    [Fact]
    public void TestSoundDefault()
    {
        Notification notification = new NotificationBuilder()
            .PlayDefaultSound()
            .Build();
    
        Assert.Equal("default", notification.SoundName);
    }
    
    [Theory]
    [InlineData("default")]
    [InlineData("robot")]
    [InlineData("space")]
    [InlineData("pinball")]
    public void TestSound(string sound)
    {
        Notification notification = new NotificationBuilder()
            .PlaySound(sound)
            .Build();
    
        Assert.Equal(sound, notification.SoundName);
        
    }
    
    [Fact]
    public void TestSoundConflict()
    {
        Assert.Throws<SoundConflictException>(() =>
        {
            Notification notification = new NotificationBuilder()
                .PlayDefaultSound()
                .PlaySound(name: "shoot in the dark", isCritical: true, volume: Volume.High)
                .Build();
        });
    }
}