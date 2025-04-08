using System.Security.Cryptography.X509Certificates;

namespace Apns.Entities;

public struct ApnsSettings
{
    public string Host { get; internal set; }
    public ApnsCertificate? Certificate { get; internal set; }
    public ApnsJsonToken? JsonToken { get; internal set; }

    public bool IsTokenAuthorizationBased => JsonToken != null;
    public bool IsCertificateAuthorizationBased => Certificate != null;
}