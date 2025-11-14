using Fitomad.Apns.Entities.Notification;
using Fitomad.Apns.Exceptions;

namespace Fitomad.Apns.Test;

public class NotificationBuilderTests
{
    [Fact]
    [Trait("CI", "TRUE")]
    public void TestSilentNotificationErrorAlert()
    {
        Assert.Throws<SilentNotificationConflictException>(() =>
        {
            var simpleAlert = new Alert
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
    [Trait("CI", "TRUE")]
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
    [Trait("CI", "TRUE")]
    public void TestSilentNotificationSound()
    {
        Assert.Throws<SilentNotificationConflictException>(() =>
        {
            var sampleSound = new SoundSettings
            {
                Critial = 1,
                Name = "custom-sound",
                Volume = Volume.High.Decibels
            };
            
            new NotificationBuilder()
                .PlaySound(sampleSound)
                .AsSilentNotification()
                .Build();
        });
    }
    
    [Fact]
    [Trait("CI", "TRUE")]
    public void TestSilentNotification()
    {
        Notification notification = new NotificationBuilder()
                .AsSilentNotification()
                .Build();
        
        Assert.Equal(notification.ContentAvailable, 1);
    }
    
    [Fact]
    [Trait("CI", "TRUE")]
    public void TestSoundDefault()
    {
        Notification notification = new NotificationBuilder()
            .PlayDefaultSound()
            .Build();
    
        Assert.Equal("default", notification.Sound);
    }
    
    [Theory]
    [InlineData("default")]
    [InlineData("robot")]
    [InlineData("space")]
    [InlineData("pinball")]
    [Trait("CI", "TRUE")]
    public void TestSound(string sound)
    {
        Notification notification = new NotificationBuilder()
            .PlaySound(sound)
            .Build();
    
        Assert.Equal(sound, notification.Sound);
        
    }
    
    [Fact]
    [Trait("CI", "TRUE")]
    public void TestSoundSetting()
    {
        Notification notification = new NotificationBuilder()
            .PlaySound(name: "sound", isCritical: true, volume: Volume.Full)
            .Build();

        Assert.IsType<SoundSettings>(notification.Sound);
        Assert.Equal("sound", (notification.Sound as SoundSettings).Name);
        
    }
}