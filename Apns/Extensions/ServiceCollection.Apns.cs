using Fitomad.Apns.Entities.Settings;
using Fitomad.Apns.Services.BearerToken;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;

namespace Fitomad.Apns.Extensions;

public static class ServiceCollectionApns
{
    private const string ApnsBaseUrl = "3/device";
    private const string ApnsTopicHeader = "apns-topic";
    
    public static void AddApns(this IServiceCollection services, ApnsSettings settings)
    {
        var httpClientBuilder = services.AddHttpClient<IApnsClient, ApnsClient>((serviceProvider, client) =>
        {
            var apnsBaseAddress = $"https://{settings.Host}/{ApnsBaseUrl}/";
            client.BaseAddress = new Uri(apnsBaseAddress);
            client.DefaultRequestVersion = HttpVersion.Version20;
            client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
            
            var jsonMediaType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(jsonMediaType);
            client.DefaultRequestHeaders.Add(ApnsTopicHeader, settings.Topic);
        });

        switch (settings) 
        {
            case { IsTokenAuthorizationBased: true, JsonToken: ApnsJsonToken jsonToken }:
                services.AddSingleton<IBearerTokenService, BearerTokenService>(
                    x => new(settings, x.GetRequiredService<IDistributedCache>())
                    );
                break;
            case { IsCertificateAuthorizationBased: true, Certificate: ApnsCertificate certificate }:
                httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler()
                {
                    SslOptions =
                    {
                        ApplicationProtocols = new() { SslApplicationProtocol.Http2 },
                        EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
                        ClientCertificates = new() { certificate.X509 },
                    }
                });
                break;
        }
    }
}