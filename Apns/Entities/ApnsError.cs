using System.Text.Json.Serialization;

namespace Apns.Entities;

public record ApnsError(int StatusCode, string Key, string Reason)
{
    // No error, response OK
    public static readonly ApnsError None = new ApnsError(0, string.Empty, string.Empty);
    public static readonly ApnsError Unknown = new(1, "Unknown", "The Apns server returns an error code not available in our code base.");
    // Error definitions
    public static readonly ApnsError BadCollapseId = new(400, "BadCollapseId", "The collapse identifier exceeds the maximum allowed size.");
    public static readonly ApnsError BadDeviceToken = new(400, "BadDeviceToken", "The specified device token is invalid. Verify that the request contains a valid token and that the token matches the environment.");
    public static readonly ApnsError BadExpirationDate = new(400, "BadExpirationDate", "The apns-expiration value is invalid.");
    public static readonly ApnsError BadMessageId = new(400, "BadMessageId", "The apns-id value is invalid.");
    public static readonly ApnsError BadPriority = new(400, "BadPriority", "The apns-priority value is invalid.");
    public static readonly ApnsError BadTopic = new(400, "BadTopic", "The apns-topic value is invalid.");
    public static readonly ApnsError DeviceTokenNotForTopic = new(400, "DeviceTokenNotForTopic", "The device token doesn’t match the specified topic.");
    public static readonly ApnsError DuplicateHeaders = new(400, "DuplicateHeaders", "One or more headers are repeated.");
    public static readonly ApnsError IdleTimeout = new(400, "IdleTimeout", "Idle timeout.");
    public static readonly ApnsError InvalidPushType = new(400, "InvalidPushType", "The apns-push-type value is invalid.");
    public static readonly ApnsError MissingDeviceToken = new(400, "MissingDeviceToken", "The device token isn’t specified in the request :path. Verify that the :path header contains the device token.");
    public static readonly ApnsError MissingTopic = new(400, "MissingTopic", "The apns-topic header of the request isn’t specified and is required.");
    public static readonly ApnsError PayloadEmpty = new(400, "PayloadEmpty", "The message payload is empty.");
    public static readonly ApnsError TopicDisallowed = new(400, "TopicDisallowed", "Pushing to this topic is not allowed.");
    public static readonly ApnsError BadCertificate = new(403, "BadCertificate", "The certificate is invalid.");
    public static readonly ApnsError BadCertificateEnvironment = new(403, "BadCertificateEnvironment", "The client certificate is for the wrong environment.");
    public static readonly ApnsError ExpiredProviderToken = new(403, "ExpiredProviderToken", "The provider token is stale and a new token should be generated.");
    public static readonly ApnsError Forbidden = new(403, "Forbidden", "The specified action is not allowed.");
    public static readonly ApnsError InvalidProviderToken = new(403, "InvalidProviderToken", "The provider token is not valid.");
    public static readonly ApnsError MissingProviderToken = new(403, "MissingProviderToken", "No provider certificate was used to connect to APNs.");
    public static readonly ApnsError UnrelatedKeyIdInToken = new(403, "UnrelatedKeyIdInToken", "The key ID in the provider token isn’t related to the key ID of the token used in the first push of this connection.");
    public static readonly ApnsError BadPath = new(404, "BadPath", "The request contained an invalid :path value.");
    public static readonly ApnsError MethodNotAllowed = new(405, "MethodNotAllowed", "The specified :method value isn’t POST.");
    public static readonly ApnsError ExpiredToken = new(410, "ExpiredToken", "The device token has expired.");
    public static readonly ApnsError Unregistered = new(410, "Unregistered", "The device token is inactive for the specified topic.");
    public static readonly ApnsError PayloadTooLarge = new(413, "PayloadTooLarge", "The message payload is too large.");
    public static readonly ApnsError TooManyProviderTokenUpdates = new(429, "TooManyProviderTokenUpdates", "The provider’s authentication token is being updated too often.");
    public static readonly ApnsError TooManyRequests = new(429, "TooManyRequests", "Too many requests were made consecutively to the same device token.");
    public static readonly ApnsError InternalServerError = new(500, "InternalServerError", "An internal server error occurred.");
    public static readonly ApnsError ServiceUnavailable = new(503, "ServiceUnavailable", "The service is unavailable.");
    public static readonly ApnsError Shutdown = new(503, "Shutdown", "The APNs server is shutting down.");

    public static ApnsError FromContent(ApnsErrorContent content)
    {
        ApnsError[] errors =
        [
            BadCollapseId,
            BadDeviceToken,
            BadExpirationDate,
            BadMessageId,
            BadPriority,
            BadTopic,
            DeviceTokenNotForTopic,
            DuplicateHeaders,
            IdleTimeout,
            InvalidPushType,
            MissingDeviceToken,
            MissingTopic,
            PayloadEmpty,
            TopicDisallowed,
            BadCertificate,
            BadCertificateEnvironment,
            ExpiredProviderToken,
            Forbidden,
            InvalidProviderToken,
            MissingProviderToken,
            UnrelatedKeyIdInToken,
            BadPath,
            MethodNotAllowed,
            ExpiredToken,
            Unregistered,
            PayloadTooLarge,
            TooManyProviderTokenUpdates,
            TooManyRequests,
            InternalServerError,
            ServiceUnavailable,
            Shutdown
        ];

        try
        {
            ApnsError currentError = errors.Where(error => error.Reason == content.Reason)
                .SingleOrDefault<ApnsError>();
        
            return currentError;
        }
        catch
        {
            return ApnsError.Unknown;
        }
        
    }
}

public struct ApnsErrorContent
{
    [JsonPropertyName("reason")]
    public string Reason { get; init; }
}