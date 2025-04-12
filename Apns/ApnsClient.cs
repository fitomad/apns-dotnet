using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Apns.Entities;
using Apns.Entities.Notification;
using Apns.Validation;

namespace Apns;

public interface IApnsClient
{
   Task<ApnsResponse> SendAsync(Notification notification, NotificationSettings notificationSettings, string deviceToken); 
   Task<ApnsResponse> SendAsync(Notification notification, string deviceToken);
}

public class ApnsClient: IApnsClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    private const string ApnsPushTypeHeader = "apns-push-type";
    private const string ApnsIdHeader = "apns-id";
    private const string ApnsExpirationHeader = "apns-expiration";
    private const string ApnsPriorityHeader = "apns-priority";
    private const string ApnsCollapseIdHeader = "apns-collapse-id";
    
    public ApnsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        _serializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
    
    public async Task<ApnsResponse> SendAsync(Notification notification, NotificationSettings settings, string deviceToken)
    {
        var notificationContainer = new NotificationContainer
        {
            Notification = notification
        };
        
        AddHttpHeaders(settings);
        
        var payload = JsonSerializer.Serialize(notificationContainer, options: _serializerOptions);
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

    private void AddHttpHeaders(NotificationSettings settings)
    {
        new Rule()
            .Property(settings.PushType).IsNotNull()
            .OnSuccess(() => _httpClient.DefaultRequestHeaders.Add(ApnsPushTypeHeader, settings.PushType.GetApnsString()))
            .Validate();

        new Rule()
            .Check(() => !string.IsNullOrEmpty(settings.NotificationId))
            .OnSuccess(() => _httpClient.DefaultRequestHeaders.Add(ApnsIdHeader, settings.NotificationId))
            .Validate();

        new Rule()
            .Property(settings.ExpirationTime).IsNotNull()
            .OnSuccess(() => _httpClient.DefaultRequestHeaders.Add(ApnsExpirationHeader, settings.ExpirationTime.ToString()))
            .Validate();

        new Rule()
            .Property(settings.Priority).IsNotNull()
            .OnSuccess(() => _httpClient.DefaultRequestHeaders.Add(ApnsPriorityHeader, settings.Priority.GetApnsString()))
            .Validate();

        new Rule()
            .Check(() => !string.IsNullOrEmpty(settings.CollapseId))
            .OnSuccess(() => _httpClient.DefaultRequestHeaders.Add(ApnsCollapseIdHeader, settings.CollapseId))
            .Validate();
    }
}