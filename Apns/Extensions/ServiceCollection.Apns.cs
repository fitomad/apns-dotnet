using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Fitomad.Apns.Entities.Settings;
using Fitomad.Apns.Services.BearerToken;

namespace Fitomad.Apns.Extensions;

public static class ServiceCollectionApns
{
    private const string ApnsAuthorizationHeader = "Bearer";
    private const string ApnsBaseUrl = "3/device";
    private const string ApnsTopicHeader = "apns-topic";
    
    public static void AddApns(this IServiceCollection services, ApnsSettings settings)
    {
        var httpClientBuilder = services.AddHttpClient<IApnsClient, ApnsClient>((serviceProvider, client) =>
        {
            var apnsBaseAddress = $"https://{settings.Host}/{ApnsBaseUrl}/";
            client.BaseAddress = new Uri(apnsBaseAddress);
            client.DefaultRequestVersion = HttpVersion.Version20;
            
            var jsonMediaType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(jsonMediaType);
            client.DefaultRequestHeaders.Add(ApnsTopicHeader, settings.Topic);

            if(settings is { IsTokenAuthorizationBased: true, JsonToken: ApnsJsonToken jsonToken })
            {
                var bearerTokenService = new BearerTokenService();
                string bearerToken = bearerTokenService.MakeBearerToken(jsonToken);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ApnsAuthorizationHeader, bearerToken);
            }
        });

        if(settings.IsCertificateAuthorizationBased)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpClientHandler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
            httpClientHandler.ClientCertificates.Add(settings.Certificate?.X509);
            
            httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);
        }
    }
}