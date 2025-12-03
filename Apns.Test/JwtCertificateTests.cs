using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fitomad.Apns.Extensions;
using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Notification;

namespace Fitomad.Apns.Test;

public class JwtCertificateTests
{
    private readonly IApnsClient _client;
    private readonly string _deviceToken;

    public JwtCertificateTests()
    {
        var userSecretsConfiguration = new ConfigurationBuilder()
            .AddUserSecrets<ClientTests>()
            .Build();

        string keyPath = userSecretsConfiguration.GetValue<string>("Apns:JwtKeyPath");
        string keyId = userSecretsConfiguration.GetValue<string>("Apns:KeyId");
        string teamId = userSecretsConfiguration.GetValue<string>("Apns:TeamId");
        
        var testSettings = new ApnsSettingsBuilder()
            .InEnvironment(ApnsEnvironment.Development)
            .SetTopic("com.desappstre.Smarty")
            .WithPathToJsonToken(keyPath, keyId, teamId)
            .Build();

        var services = new ServiceCollection();
        services.AddApns(settings: testSettings);
        var provider = services.BuildServiceProvider();
        
        _client = provider.GetRequiredService<IApnsClient>();
        _deviceToken = userSecretsConfiguration.GetValue<string>("Apns:DeviceToken");
    }

    [Fact]
    [Trait("CI", "FALSE")]
    public async Task TestSimpleNotification()
    {
        var alert = new Alert
        {
            Title = "Jwt Token Notification",
            Body = "This notification has been sent using APNS signed with a JWT Key"
        };
        
        var notification = new NotificationBuilder()
            .WithAlert(alert)
            .Build();

        ApnsResponse response = await _client.SendAsync(new NotificationContainer
        { Notification = notification }, deviceToken: _deviceToken);

        Assert.True(response.IsSuccess);
    }
}