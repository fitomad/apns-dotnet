using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fitomad.Apns.Extensions;
using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Notification;

namespace Fitomad.Apns.Test;

public class ClientTests
{
    private readonly IApnsClient _client;
    private readonly string _deviceToken;
    
    public static IEnumerable<object[]> InterruptionLevels
    {
        get
        {
            yield return new object[] { InterruptionLevel.Passive };
            yield return new object[] { InterruptionLevel.Active };
            yield return new object[] { InterruptionLevel.Critical };
            yield return new object[] { InterruptionLevel.TimeSensitive };
        }
    }
    public ClientTests()
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
    [Trait("CI", "FALSE")]
    public async Task TestConnection()
    {
        var alertContent = new Alert()
        {
            Title = "Test Alert",
            Subtitle = "Test Subtitle",
            Body = "Test Body"
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .Build();
        
       ApnsResponse apnsResponse = await _client.SendAsync(new NotificationContainer 
       { Notification = notification }, deviceToken: _deviceToken);
       
        Assert.True(apnsResponse.IsSuccess);
        Assert.NotNull(apnsResponse.Guid);
        Assert.NotEmpty(apnsResponse.Guid.ApnsId);
        Assert.NotEmpty(apnsResponse.Guid.ApnsUniqueId);
    }
    
    [Fact]
    [Trait("CI", "FALSE")]
    public async Task TestLocalizableNotification()
    {
        var alertContent = new LocalizableAlert()
        {
            TitleLocalizationKey = "push_title",
            SubtitleLocalizationKey = "push_subtitle",
            BodyLocalizationKey = "push_body"
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .Build();

        ApnsResponse apnsResponse = await _client.SendAsync(new NotificationContainer
        { Notification = notification }, deviceToken: _deviceToken);

        Assert.True(apnsResponse.IsSuccess);   
        Assert.NotNull(apnsResponse.Guid);
        Assert.NotEmpty(apnsResponse.Guid.ApnsId);
        Assert.NotEmpty(apnsResponse.Guid.ApnsUniqueId);
    }
    
    [Fact]
    [Trait("CI", "FALSE")]
    public async Task TestLocalizableArgumentsNotification()
    {
        var alertContent = new LocalizableAlert()
        {
            TitleLocalizationKey = "push_title_arg",
            TitleLocalizationArguments = [ "España" ],
            SubtitleLocalizationKey = "push_subtitle_arg",
            SubtitleLocalizationArguments = [ "Madrid" ],
            BodyLocalizationKey = "push_body_arg",
            BodyLocalizationArguments = [ "❤️" ]
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .Build();

        ApnsResponse apnsResponse = await _client.SendAsync(new NotificationContainer
        { Notification = notification }, deviceToken: _deviceToken);

        Assert.True(apnsResponse.IsSuccess); 
        Assert.NotNull(apnsResponse.Guid);
        Assert.NotEmpty(apnsResponse.Guid.ApnsId);
        Assert.NotEmpty(apnsResponse.Guid.ApnsUniqueId);
    }
    
    [Fact]
    [Trait("CI", "FALSE")]
    public async Task TestBadge()
    {
        var alertContent = new LocalizableAlert()
        {
            TitleLocalizationKey = "push_title",
            SubtitleLocalizationKey = "push_subtitle",
            BodyLocalizationKey = "push_body"
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .WithBadgeCount(1)
            .Build();

        ApnsResponse apnsResponse = await _client.SendAsync(new NotificationContainer
        { Notification = notification }, deviceToken: _deviceToken);

        Assert.True(apnsResponse.IsSuccess);
        Assert.NotNull(apnsResponse.Guid);
        Assert.NotEmpty(apnsResponse.Guid.ApnsId);
        Assert.NotEmpty(apnsResponse.Guid.ApnsUniqueId);
    }
    
    [Fact]
    [Trait("CI", "FALSE")]
    public async Task TestBadgeRemove()
    {
        var alertContent = new LocalizableAlert()
        {
            TitleLocalizationKey = "push_title",
            SubtitleLocalizationKey = "push_subtitle",
            BodyLocalizationKey = "push_body"
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .WithBadgeCount(0)
            .Build();

        ApnsResponse apnsResponse = await _client.SendAsync(new NotificationContainer
        { Notification = notification }, deviceToken: _deviceToken);

        Assert.True(apnsResponse.IsSuccess);   
        Assert.NotNull(apnsResponse.Guid);
        Assert.NotEmpty(apnsResponse.Guid.ApnsId);
        Assert.NotEmpty(apnsResponse.Guid.ApnsUniqueId);
    }
    
    [Fact]
    [Trait("CI", "FALSE")]
    public async Task TestBadgeRemoveWithDedicatedMethod()
    {
        var alertContent = new LocalizableAlert()
        {
            TitleLocalizationKey = "push_title",
            SubtitleLocalizationKey = "push_subtitle",
            BodyLocalizationKey = "push_body"
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .ClearBadgeCount()
            .Build();

        ApnsResponse apnsResponse = await _client.SendAsync(new NotificationContainer
        { Notification = notification }, deviceToken: _deviceToken);

        Assert.True(apnsResponse.IsSuccess); 
        Assert.NotNull(apnsResponse.Guid);
        Assert.NotEmpty(apnsResponse.Guid.ApnsId);
        Assert.NotEmpty(apnsResponse.Guid.ApnsUniqueId);
    }

    [Theory]
    [MemberData(nameof(InterruptionLevels))]
    [Trait("CI", "FALSE")]
    public async Task TestInterruptionLevel(InterruptionLevel level)
    {
        var alertContent = new LocalizableAlert()
        {
            TitleLocalizationKey = "push_title_arg",
            TitleLocalizationArguments = [ level.Value ],
            SubtitleLocalizationKey = "push_subtitle_arg",
            SubtitleLocalizationArguments = [ level.Value ],
            BodyLocalizationKey = "push_body_arg",
            BodyLocalizationArguments = [ level.Value ]
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .SetInterruptionLevel(level)
            .Build();

        Assert.Equal(notification.InterruptionLevel, level.Value);

        ApnsResponse apnsResponse = await _client.SendAsync(new NotificationContainer
        { Notification = notification }, deviceToken: _deviceToken);

        Assert.True(apnsResponse.IsSuccess); 
        Assert.NotNull(apnsResponse.Guid);
        Assert.NotEmpty(apnsResponse.Guid.ApnsId);
        Assert.NotEmpty(apnsResponse.Guid.ApnsUniqueId);
    }
    
    [Theory]
    [InlineData(0.0)]
    [InlineData(0.10)]
    [InlineData(0.25)]
    [InlineData(0.50)]
    [InlineData(0.75)]
    [InlineData(1.0)]
    [Trait("CI", "FALSE")]
    public async Task TestRelevance(double relevance)
    {
        var alertContent = new LocalizableAlert()
        {
            TitleLocalizationKey = "push_title_arg",
            TitleLocalizationArguments = [ relevance ],
            SubtitleLocalizationKey = "push_subtitle_arg",
            SubtitleLocalizationArguments = [ relevance ],
            BodyLocalizationKey = "push_body_arg",
            BodyLocalizationArguments = [ relevance ]
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .SetRelevanceScore(relevance)
            .Build();

        Assert.Equal(notification.RelevanceScore, relevance);

        ApnsResponse apnsResponse = await _client.SendAsync(new NotificationContainer
        { Notification = notification }, deviceToken: _deviceToken);

        Assert.True(apnsResponse.IsSuccess); 
        Assert.NotNull(apnsResponse.Guid);
        Assert.NotEmpty(apnsResponse.Guid.ApnsId);
        Assert.NotEmpty(apnsResponse.Guid.ApnsUniqueId);
    }
    
    [Theory]
    [InlineData("Grupo Uno")]
    [InlineData("Grupo Dos")]
    [InlineData("Grupo Tres")]
    [Trait("CI", "FALSE")]
    public async Task TestThreadIdGroup(string group)
    {
        var alertContent = new LocalizableAlert()
        {
            TitleLocalizationKey = "push_title_arg",
            TitleLocalizationArguments = [ group ],
            SubtitleLocalizationKey = "push_subtitle_arg",
            SubtitleLocalizationArguments = [ group ],
            BodyLocalizationKey = "push_body_arg",
            BodyLocalizationArguments = [ group ]
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .WithThreadId(group)
            .Build();

        Assert.Equal(notification.ThreadId, group);

        ApnsResponse apnsResponse = await _client.SendAsync(new NotificationContainer
        { Notification = notification }, deviceToken: _deviceToken);

        Assert.True(apnsResponse.IsSuccess); 
        Assert.NotNull(apnsResponse.Guid);
        Assert.NotEmpty(apnsResponse.Guid.ApnsId);
        Assert.NotEmpty(apnsResponse.Guid.ApnsUniqueId);
    }
    
    [Fact]
    [Trait("CI", "FALSE")]
    public async Task TestAllowContentModification()
    {
        var alertContent = new Alert()
        {
            Title = "Test Content Modification",
            Subtitle = "Test Subtitle",
            Body = "Test Body"
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .EnableAppExtensionModification()
            .Build();

        Assert.Equal(notification.MutableContent, 1);

        ApnsResponse apnsResponse = await _client.SendAsync(new NotificationContainer
        { Notification = notification }, deviceToken: _deviceToken);

        Assert.True(apnsResponse.IsSuccess); 
        Assert.NotNull(apnsResponse.Guid);
        Assert.NotEmpty(apnsResponse.Guid.ApnsId);
        Assert.NotEmpty(apnsResponse.Guid.ApnsUniqueId);
    }
}