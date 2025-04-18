using Fitomad.Apns.Entities.Settings;

namespace Fitomad.Apns.Services.BearerToken;

public interface IBearerTokenService
{
    string MakeBearerToken(ApnsJsonToken jsonToken);
}