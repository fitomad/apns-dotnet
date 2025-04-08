using System.Configuration;
using Apns.Entities;
using Apns.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apns.Test;

public class ClientTests
{
    private string _apiKey;
    private ApnsClient _client;
    
    public ClientTests()
    {
        var testSettings = new ApnsSettingsBuilder()
            .InEnvironment(ApnsEnvironment.Development)
            .WithPathToX509Certificate2("/Users/adolfo/Documents/Proyectos/Packages/ApnsCertificates/apns-smarty.cer")
            .Build();

        var services = new ServiceCollection();
        services.AddApns(settings: testSettings);
        var provider = services.BuildServiceProvider();
        
        _client = provider.GetRequiredService<ApnsClient>();
    }

    [Fact]
    public async Task TestConnection()
    {
        var token = "fake-token";
        //var testNotification = Noti
        //var apnsResponse = await _client.SendAsync(notification: , deviceToken: token);
    }
}