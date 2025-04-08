using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Apns.Entities;

namespace Apns.Extensions;

public static class ServiceCollectionApns
{
    private static readonly string ApnsAuthorizationHeader = "Bearer";
    private static readonly string ApnsBaseUrl = "/3/device";
    
    public static void AddApns(this IServiceCollection services, ApnsSettings settings)
    {
        var httpClientBuilder = services.AddHttpClient<IApnsClient, ApnsClient>(client =>
        {
            client.BaseAddress = new Uri($"https://{settings.Host}/{ApnsBaseUrl}");
            client.DefaultRequestVersion = HttpVersion.Version20;
            
            var jsonMediaType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(jsonMediaType);

            if(settings.IsTokenAuthorizationBased)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ApnsAuthorizationHeader, settings.JsonToken.TeamId);
            }
        });

        if(settings.IsCertificateAuthorizationBased)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpClientHandler.ClientCertificates.Add(settings.Certificate.X509);
            
            httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);
        }
    }
}