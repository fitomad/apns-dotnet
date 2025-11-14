using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Notification;
using Fitomad.Apns.Exceptions;
using Fitomad.Apns.Services.Validation;

namespace Fitomad.Apns;

public class ApnsClient: IApnsClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    private const string ApnsPushTypeHeader = "apns-push-type";
    private const string ApnsIdHeader = "apns-id";
    private const string ApnsUniqueIdHeader = "apns-unique-id";
    private const string ApnsExpirationHeader = "apns-expiration";
    private const string ApnsPriorityHeader = "apns-priority";
    private const string ApnsCollapseIdHeader = "apns-collapse-id";
    private const string ApnsTopicHeader = "apns-topic";
    
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

        string? uniqueId;
        try
        {
            uniqueId = response.Headers.GetValues(ApnsUniqueIdHeader).First();
        }
        catch (InvalidOperationException)
        {
            uniqueId = null;
        }

        string id = response.Headers.GetValues(ApnsIdHeader).First();
        var apnsGuid = new ApnsGuid(id, uniqueId);
        
        return ApnsResponse.Success(apnsGuid);
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
            .Property(settings.PushType).IsNotNull()
            .Property(settings.PushType).IsEqualsTo(NotificationType.LiveActivity)
            .OnSuccess(() =>
            {
                var baseTopicHeader = _httpClient.DefaultRequestHeaders.GetValues(ApnsTopicHeader).First<string>();
                var liveActivityTopicHeader = $"{baseTopicHeader}.push-type.liveactivity";
                    
                _httpClient.DefaultRequestHeaders.Add(ApnsTopicHeader, liveActivityTopicHeader);
            })
            .Validate();
        
        new Rule()
            .Property(settings.PushType).IsNotNull()
            .Property(settings.PushType).IsEqualsTo(NotificationType.VoIp)
            .OnSuccess(() =>
            {
                var baseTopicHeader = _httpClient.DefaultRequestHeaders.GetValues(ApnsTopicHeader).First<string>();
                var voipTopicHeader = $"{baseTopicHeader}.voip";
                    
                _httpClient.DefaultRequestHeaders.Add(ApnsTopicHeader, voipTopicHeader);
            })
            .Validate();

        new Rule()
            .Property(settings.NotificationId).IsNotNull()
            .Property(settings.NotificationId).MatchRegularExpression(@"^\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$")
            .OnSuccess(() => _httpClient.DefaultRequestHeaders.Add(ApnsIdHeader, settings.NotificationId))
            .OnFailure(() => throw new ApnsIdHeaderNonValidException())
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
            .Property(settings.CollapseId).IsNotNull()
            .OnSuccess(() => _httpClient.DefaultRequestHeaders.Add(ApnsCollapseIdHeader, settings.CollapseId))
            .Validate();
    }
}