using Microsoft.Extensions.DependencyInjection;
using Apns.Entities;
using Apns.Entities.Notification;
using Apns.Extensions;

namespace Apns.Test;

public class ClientTests
{
    private readonly IApnsClient _client;

    private const string DeviceToken = "809d883e809438d27c53c17089eccde5cec69447c72ceea5234dcc23bb9340fff356969cf17b5e56b54336ce1ee9ef2488eefea96a97eb7050322fd748ceca72a24dc8ff83cb63ae3b8bfb1dea35b1e8";
    
    public ClientTests()
    {
        var testSettings = new ApnsSettingsBuilder()
            .InEnvironment(ApnsEnvironment.Development)
            .SetTopic("com.desappstre.Smarty")
            .WithPathToX509Certificate2("/Users/adolfo/Documents/Proyectos/Packages/ApnsCertificates/apns-smarty.p12", "12345678")
            .Build();

        var services = new ServiceCollection();
        services.AddApns(settings: testSettings);
        var provider = services.BuildServiceProvider();
        
        _client = provider.GetRequiredService<IApnsClient>();
    }

    [Fact]
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
        
       ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: DeviceToken);
       
        Assert.True(apnsResponse.IsSuccess);   
    }
    
    [Fact]
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
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: DeviceToken);

        Assert.True(apnsResponse.IsSuccess);   
    }
    
    [Fact]
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
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: DeviceToken);

        Assert.True(apnsResponse.IsSuccess);   
    }
    
    [Fact]
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
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: DeviceToken);

        Assert.True(apnsResponse.IsSuccess);   
    }
    
    [Fact]
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
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: DeviceToken);

        Assert.True(apnsResponse.IsSuccess);   
    }
    
    [Fact]
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
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: DeviceToken);

        Assert.True(apnsResponse.IsSuccess);   
    }

    [Theory]
    [InlineData(InterruptionLevel.Passive, "passive")]
    [InlineData(InterruptionLevel.Active, "active")]
    [InlineData(InterruptionLevel.TimeSensitive, "time-sensitive")]
    [InlineData(InterruptionLevel.Critical, "critical")]
    public async Task TestInterruptionLevel(InterruptionLevel level, string expectedLevel)
    {
        var alertContent = new LocalizableAlert()
        {
            TitleLocalizationKey = "push_title_arg",
            TitleLocalizationArguments = [ expectedLevel ],
            SubtitleLocalizationKey = "push_subtitle_arg",
            SubtitleLocalizationArguments = [ expectedLevel ],
            BodyLocalizationKey = "push_body_arg",
            BodyLocalizationArguments = [ expectedLevel ]
        };

        Notification notification = new NotificationBuilder()
            .WithAlert(alertContent)
            .SetInterruptionLevel(level)
            .Build();

        Assert.Equal(notification.InterruptionLevel, expectedLevel);
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: DeviceToken);

        Assert.True(apnsResponse.IsSuccess); 
    }
    
    [Theory]
    [InlineData(0.0)]
    [InlineData(0.10)]
    [InlineData(0.25)]
    [InlineData(0.50)]
    [InlineData(0.75)]
    [InlineData(1.0)]
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
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: DeviceToken);

        Assert.True(apnsResponse.IsSuccess); 
    }
    
    [Theory]
    [InlineData("Grupo Uno")]
    [InlineData("Grupo Dos")]
    [InlineData("Grupo Tres")]
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
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: DeviceToken);

        Assert.True(apnsResponse.IsSuccess); 
    }
    
    [Fact]
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
        
        ApnsResponse apnsResponse = await _client.SendAsync(notification, deviceToken: DeviceToken);

        Assert.True(apnsResponse.IsSuccess); 
    }
}