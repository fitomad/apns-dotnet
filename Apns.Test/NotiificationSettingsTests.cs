using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fitomad.Apns.Extensions;
using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Notification;
using Fitomad.Apns.Exceptions;

namespace Fitomad.Apns.Test;

public class NotificationSettingsTests
{
    private readonly IApnsClient _client;
    private readonly string _deviceToken;
    public NotificationSettingsTests()
    {
        var userSecretsConfiguration = new ConfigurationBuilder()
            .AddUserSecrets<ClientTests>()
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
    public async Task TestApnsIdSetting()
    {
        var testSettings = new NotificationSettings
        {
            NotificationId = (new Guid()).ToString()
        }; 
        var alertContent = new Alert()
        {
            Title = "Test Apns-Id header",
            Body = "Setting the apns-id header."
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .Build();
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, notificationSettings: testSettings, deviceToken: _deviceToken);
        
        Assert.True(apnsResponse.IsSuccess);
    }
    
    [Fact]
    public async Task TestApnsIdSettingWrongFormat()
    {
        await Assert.ThrowsAsync<ApnsIdHeaderNonValidException>(async () =>
        {
            var testSettings = new NotificationSettings
            {
                NotificationId = "NOT-AN-UUID-FORMAT"
            }; 
            var alertContent = new Alert()
            {
                Title = "Test apns-Id header",
                Body = "Setting the apns-id header."
            };

            Notification notification = new NotificationBuilder()
                .WithAlert(alertContent)
                .Build();
        
            ApnsResponse apnsResponse = await _client.SendAsync(notification, notificationSettings: testSettings, deviceToken: _deviceToken);
        });
    }

    [Fact]
    public async Task TestNotificationTypeHeader()
    {
        var testSettings = new NotificationSettings
        {
            PushType = NotificationType.Alert
        };

        var alertContent = new Alert
        {
            Title = "Test apns-push-type header",
            Subtitle = "Test apns-push-type header"
        };
        
        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .Build();
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, notificationSettings: testSettings, deviceToken: _deviceToken);
        
        Assert.True(apnsResponse.IsSuccess);
    }
    
    [Fact]
    public async Task TestNotificationWrongTypeHeader()
    {
        var testSettings = new NotificationSettings
        {
            PushType = NotificationType.LiveActivity
        };

        var alertContent = new Alert
        {
            Title = "Test apns-push-type header",
            Subtitle = "Test apns-push-type header"
        };
        
        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .Build();
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, notificationSettings: testSettings, deviceToken: _deviceToken);
        
        Assert.False(apnsResponse.IsSuccess);
    }

    [Fact]
    public async Task TestNotificationExpiration()
    {
        var testSettings = new NotificationSettings
        {
            ExpirationTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()
        };

        var alertContent = new Alert
        {
            Title = "Test apns-expiration header",
            Subtitle = "Test apns-expiration header"
        };
        
        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .Build();
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, notificationSettings: testSettings, deviceToken: _deviceToken);
        
        Assert.True(apnsResponse.IsSuccess);
    }
    
    [Fact]
    public async Task TestNotificationCollapseId()
    {
        var testSettings = new NotificationSettings
        {
            CollapseId = "The collapse id test"
        };

        var alertContent = new Alert
        {
            Title = "Test apns-collapse-id header",
            Subtitle = "Test apns-collapse-id header"
        };
        
        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .Build();
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, notificationSettings: testSettings, deviceToken: _deviceToken);
        
        Assert.True(apnsResponse.IsSuccess);
    }
}