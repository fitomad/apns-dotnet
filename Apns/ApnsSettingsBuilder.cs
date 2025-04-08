using System.Security.Cryptography.X509Certificates;
using Apns.Entities;

namespace Apns;

public class EnvironmentNotSetException : Exception
{
    private const string EnvironmentMessage = "Environment not set";
    
    public EnvironmentNotSetException() : base(message: EnvironmentMessage)
    {
    }
}

public class AuthorizationNotSetException : Exception
{
    private const string AuthorizationNotSetMessage = "Environment not set";
    
    public AuthorizationNotSetException() : base(message: AuthorizationNotSetMessage)
    {
    }
}

public class DuplicatedAuthorizationException : Exception
{
    private const string DuplicatedAuthorizationMessage = "Environment not set";
    
    public DuplicatedAuthorizationException() : base(message: DuplicatedAuthorizationMessage)
    {
    }
}

public record ApnsJsonToken
{
    private readonly string _token;
    
    public string KeyId { get; internal set; }
    public string BundleId { get; internal set; }
    public string TeamId { get; internal set; }

    public ApnsJsonToken(string content)
    {
        _token = content;
    }
}

public record ApnsCertificate
{
    public X509Certificate2 X509 { get; init; }

    public ApnsCertificate(X509Certificate2 x509)
    {
        X509 = x509;
    }

    public ApnsCertificate(string pathToCertificate)
    {
        X509 = X509CertificateLoader.LoadCertificateFromFile(pathToCertificate);
    }
}

public interface IApnsSettingsBuilder
{
    // Environments
    IApnsSettingsBuilder InEnvironment(ApnsEnvironment environment);
    // JWT
    IApnsSettingsBuilder WithJsonToken(ApnsJsonToken jsonToken);
    // X509 Certificate
    IApnsSettingsBuilder WithCertificate(ApnsCertificate certificate);
    IApnsSettingsBuilder WithX509Certificate2(X509Certificate2 certificate);
    IApnsSettingsBuilder WithPathToX509Certificate2(string pathToCrtificate);
    // Create new settings
    ApnsSettings Build();
}

public sealed class ApnsSettingsBuilder: IApnsSettingsBuilder
{
    private ApnsSettings _settings;

    public ApnsSettingsBuilder()
    {
        _settings = new ApnsSettings();
    }
    
    public IApnsSettingsBuilder InEnvironment(ApnsEnvironment environment)
    {
        _settings.Host = environment.GetApnsServer();
        return this;
    }
    
    public IApnsSettingsBuilder WithJsonToken(ApnsJsonToken jsonToken)
    {
        _settings.JsonToken = jsonToken;
        return this;
    }

    public IApnsSettingsBuilder WithCertificate(ApnsCertificate certificate)
    {
        _settings.Certificate = certificate;
        return this;
    }

    public IApnsSettingsBuilder WithX509Certificate2(X509Certificate2 certificate)
    {
        var apnsCertificate = new ApnsCertificate(certificate);
        _settings.Certificate = apnsCertificate;
        
        return this;
    }

    public IApnsSettingsBuilder WithPathToX509Certificate2(string pathToCrtificate)
    {
        var apnsCertificate = new ApnsCertificate(pathToCrtificate);
        _settings.Certificate = apnsCertificate;
        
        return this;
    }

    public ApnsSettings Build()
    {
        if(string.IsNullOrEmpty(_settings.Host))
        {
            // Environment is mandatory
            throw new EnvironmentNotSetException();
        }
        
        if(_settings.IsTokenAuthorizationBased && _settings.IsCertificateAuthorizationBased)
        {
            // Only JWT or Certificate valid, not both
            throw new DuplicatedAuthorizationException();
        }

        if(!_settings.IsTokenAuthorizationBased && !_settings.IsCertificateAuthorizationBased)
        {
            // You must set an authorization mechanism
            throw new AuthorizationNotSetException();
        }
        
        return _settings;
    }
}
