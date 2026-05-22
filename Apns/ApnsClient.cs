using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Notification;
using Fitomad.Apns.Exceptions;
using Fitomad.Apns.Services.BearerToken;
using Fitomad.Apns.Services.Validation;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fitomad.Apns;

public class ApnsClient : IApnsClient
{
    private readonly IBearerTokenService? _bearerTokenService;

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    private const string ApnsAuthorizationHeader = "Bearer";
    private const string ApnsPushTypeHeader = "apns-push-type";
    private const string ApnsIdHeader = "apns-id";
    private const string ApnsUniqueIdHeader = "apns-unique-id";
    private const string ApnsExpirationHeader = "apns-expiration";
    private const string ApnsPriorityHeader = "apns-priority";
    private const string ApnsCollapseIdHeader = "apns-collapse-id";
    private const string ApnsTopicHeader = "apns-topic";

    public ApnsClient(HttpClient httpClient, IBearerTokenService? bearerTokenService = null)
    {
        _bearerTokenService = bearerTokenService;

        _httpClient = httpClient;

        _serializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<ApnsResponse> SendAsync<T>(T container, NotificationSettings settings, string deviceToken)
        where T : NotificationContainer
    {
        var payload = JsonSerializer.Serialize(container, options: _serializerOptions);
        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        AddHttpHeaders(httpContent, settings);

        HttpRequestMessage request = new(HttpMethod.Post, deviceToken)
        { Content = httpContent };
        if (_bearerTokenService is not null) 
        {
            string bearerToken = _bearerTokenService.GetBearerToken();
            request.Headers.Authorization = new AuthenticationHeaderValue(ApnsAuthorizationHeader, bearerToken);
        }

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        if (response.StatusCode != HttpStatusCode.OK)
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

    public async Task<ApnsResponse> SendAsync<T>(T container, string deviceToken)
        where T : NotificationContainer
    {
        return await SendAsync(container, NotificationSettings.Default, deviceToken);
    }

    private void AddHttpHeaders(HttpContent httpContent, NotificationSettings settings)
    {
        new Rule()
            .Property(settings.PushType).IsNotNull()
            .OnSuccess(() => httpContent.Headers.Add(ApnsPushTypeHeader, settings.PushType.GetApnsString()))
            .Validate();

        new Rule()
            .Property(settings.PushType).IsNotNull()
            .Property(settings.PushType).IsEqualsTo(NotificationType.LiveActivity)
            .OnSuccess(() =>
            {
                var baseTopicHeader = httpContent.Headers.GetValues(ApnsTopicHeader).First<string>();
                var liveActivityTopicHeader = $"{baseTopicHeader}.push-type.liveactivity";

                httpContent.Headers.Add(ApnsTopicHeader, liveActivityTopicHeader);
            })
            .Validate();

        new Rule()
            .Property(settings.PushType).IsNotNull()
            .Property(settings.PushType).IsEqualsTo(NotificationType.VoIp)
            .OnSuccess(() =>
            {
                var baseTopicHeader = httpContent.Headers.GetValues(ApnsTopicHeader).First<string>();
                var voipTopicHeader = $"{baseTopicHeader}.voip";

                httpContent.Headers.Add(ApnsTopicHeader, voipTopicHeader);
            })
            .Validate();

        new Rule()
            .Property(settings.NotificationId).IsNotNull()
            .Property(settings.NotificationId).MatchRegularExpression(@"^\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$")
            .OnSuccess(() => httpContent.Headers.Add(ApnsIdHeader, settings.NotificationId))
            .OnFailure(() => throw new ApnsIdHeaderNonValidException())
            .Validate();

        new Rule()
            .Property(settings.ExpirationTime).IsNotNull()
            .OnSuccess(() => httpContent.Headers.Add(ApnsExpirationHeader, settings.ExpirationTime.ToString()))
            .Validate();

        new Rule()
            .Property(settings.Priority).IsNotNull()
            .OnSuccess(() => httpContent.Headers.Add(ApnsPriorityHeader, settings.Priority.GetApnsString()))
            .Validate();

        new Rule()
            .Property(settings.CollapseId).IsNotNull()
            .OnSuccess(() => httpContent.Headers.Add(ApnsCollapseIdHeader, settings.CollapseId))
            .Validate();
    }
}