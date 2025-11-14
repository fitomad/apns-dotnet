using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fitomad.Apns.Extensions;
using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Notification;

namespace Fitomad.Apns.Test;

public class LiveActivityNotificationTests
{
    private readonly IApnsClient _client;
    private readonly string _deviceToken;

    public LiveActivityNotificationTests()
    {
        var userSecretsConfiguration = new ConfigurationBuilder()
            .AddUserSecrets<LiveActivityNotificationTests>()
            .Build();

        string certPath = userSecretsConfiguration.GetValue<string>("Apns:CertPath");
        string certPassword = userSecretsConfiguration.GetValue<string>("Apns:CertPassword");
        
        var testSettings = new ApnsSettingsBuilder()
            .InEnvironment(ApnsEnvironment.Development)
            .SetTopic("com.desappstre.Smarty")
            .WithPathToX509Certificate2(certPath, certPassword)
            .Build();

        var services = new ServiceCollection();
        services.AddApns(settings: testSettings);
        var provider = services.BuildServiceProvider();
        
        _client = provider.GetRequiredService<IApnsClient>();
        _deviceToken = userSecretsConfiguration.GetValue<string>("Apns:DeviceToken");
    }

    [Fact]
    [Trait("CI", "FALSE")]
    public async Task TestLiveActivityStart()
    {
        var notificationSettings = new NotificationSettings
        {
            PushType = NotificationType.LiveActivity
        };

        var alert = new Alert
        {
            Title = "Live Activity Title",
            Body = "Live Activity Body"
        };

        var dataContent = new Dictionary<string, object>
        {
            { "name", "Adolfo" }
        };

        var liveActivityNotification = new NotificationBuilder()
            .WithAlert(alert)
            .UseLiveActivity()
            .SetTimestamp(DateTimeOffset.UtcNow.AddMinutes(2).ToUnixTimeSeconds())
            .SetLiveActivityEvent(LiveActivityEvent.Start)
            .SetLiveActivityAttributeType("MockAttributeType")
            .SetLiveActivityStartAttributes(dataContent)
            .WithLiveActivityContent(dataContent)
            .Build(); 
        
        ApnsResponse response = await _client.SendAsync(liveActivityNotification, notificationSettings: notificationSettings, deviceToken: _deviceToken);
        
        Assert.True(response.IsFailure);
        Assert.Equal("TopicDisallowed", response.Error.Key);
    }
    
    [Fact]
    [Trait("CI", "FALSE")]
    public async Task TestLiveActivityUpdate()
    {
        var notificationSettings = new NotificationSettings
        {
            PushType = NotificationType.LiveActivity
        };

        var alert = new Alert
        {
            Title = "Live Activity Title",
            Body = "Live Activity Body"
        };

        var dataContent = new Dictionary<string, object>
        {
            { "name", "Adolfo" }
        };

        var liveActivityNotification = new NotificationBuilder()
            .WithAlert(alert)
            .UseLiveActivity()
            .SetTimestamp(DateTimeOffset.UtcNow.AddMinutes(2).ToUnixTimeSeconds())
            .SetLiveActivityEvent(LiveActivityEvent.Update)
            .SetLiveActivityAttributeType("MockAttributeType")
            .SetLiveActivityStartAttributes(dataContent)
            .WithLiveActivityContent(dataContent)
            .Build(); 
        
        ApnsResponse response = await _client.SendAsync(liveActivityNotification, notificationSettings: notificationSettings, deviceToken: _deviceToken);
        
        Assert.True(response.IsFailure);
        Assert.Equal("TopicDisallowed", response.Error.Key);
    }
    
    [Fact]
    [Trait("CI", "FALSE")]
    public async Task TestLiveActivityEnd()
    {
        var notificationSettings = new NotificationSettings
        {
            PushType = NotificationType.LiveActivity
        };

        var alert = new Alert
        {
            Title = "Live Activity Title",
            Body = "Live Activity Body"
        };

        var dataContent = new Dictionary<string, object>
        {
            { "name", "Adolfo" }
        };

        var liveActivityNotification = new NotificationBuilder()
            .WithAlert(alert)
            .UseLiveActivity()
            .SetTimestamp(DateTimeOffset.UtcNow.AddMinutes(2).ToUnixTimeSeconds())
            .SetLiveActivityEvent(LiveActivityEvent.End)
            .SetLiveActivityAttributeType("MockAttributeType")
            .SetLiveActivityStartAttributes(dataContent)
            .WithLiveActivityContent(dataContent)
            .Build(); 
        
        ApnsResponse response = await _client.SendAsync(liveActivityNotification, notificationSettings: notificationSettings, deviceToken: _deviceToken);
        
        Assert.True(response.IsFailure);
        Assert.Equal("TopicDisallowed", response.Error.Key);
    }
}