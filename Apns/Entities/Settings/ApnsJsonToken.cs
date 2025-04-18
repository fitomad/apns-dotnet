namespace Fitomad.Apns.Entities.Settings;

public record ApnsJsonToken
{
    public string Content { get; init; }
    public string KeyId { get; init; }
    public string TeamId { get; init; }

    public long Timestamp => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
}
