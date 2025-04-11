using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Apns.Entities;
using Apns.Entities.Notification;

namespace Apns;

public interface IApnsClient
{
   Task<ApnsResponse> SendAsync(Notification notification, NotificationSettings notificationSettings, string deviceToken); 
   Task<ApnsResponse> SendAsync(Notification notification, string deviceToken);
}

public class ApnsClient: IApnsClient
{
    private readonly HttpClient _httpClient;
    
    public ApnsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<ApnsResponse> SendAsync(Notification notification, NotificationSettings settings, string deviceToken)
    {
        var notificationContainer = new NotificationContainer
        {
            Notification = notification
        };

        var serializerOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        
        var payload = JsonSerializer.Serialize(notificationContainer, options: serializerOptions);
        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await _httpClient.PostAsync(deviceToken, httpContent);

        if(response.StatusCode != HttpStatusCode.OK)
        {
            ApnsErrorContent errorContent = await response.Content.ReadFromJsonAsync<ApnsErrorContent>();
            ApnsError error = ApnsError.FromContent(errorContent);

            return ApnsResponse.Failure(error);
        }
        
        return ApnsResponse.Success();
    }

    public async Task<ApnsResponse> SendAsync(Notification notification, string deviceToken)
    {
        return await SendAsync(notification, NotificationSettings.Default, deviceToken);
    }
}