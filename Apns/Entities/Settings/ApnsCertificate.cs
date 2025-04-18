using System.Security.Cryptography.X509Certificates;

namespace Fitomad.Apns.Entities.Settings;

public record ApnsCertificate
{
    public X509Certificate2 X509 { get; }

    public ApnsCertificate(X509Certificate2 x509)
    {
        X509 = x509;
    }

    public ApnsCertificate(string pathToCertificate)
    {
        X509 = new X509Certificate2(pathToCertificate);
    }
    
    public ApnsCertificate(string pathToCertificate, string password)
    {
        X509 = new X509Certificate2(pathToCertificate, password);

    }
}