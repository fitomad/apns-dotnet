using System.Security.Cryptography.X509Certificates;
using Apns.Entities;
using Apns.Validation;

namespace Apns;

public class EnvironmentNotSetException : Exception
{
    private const string EnvironmentMessage = "Environment not set";
    
    public EnvironmentNotSetException() : base(message: EnvironmentMessage)
    {
    }
}

public class TopicNotSetException : Exception
{
    private const string TopicMessage = "Topic not set and it's mandatory to set the `apns-topic` header";
    
    public TopicNotSetException() : base(message: TopicMessage)
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
    public X509Certificate2 X509 { get; }

    public ApnsCertificate(X509Certificate2 x509)
    {
        X509 = x509;
    }

    public ApnsCertificate(string pathToCertificate)
    {
        X509 = X509CertificateLoader.LoadCertificateFromFile(pathToCertificate);
    }
    
    public ApnsCertificate(string pathToCertificate, string password)
    {
        X509 = new X509Certificate2(pathToCertificate, password);

    }
}

public interface IApnsSettingsBuilder
{
    // Environments
    IApnsSettingsBuilder InEnvironment(ApnsEnvironment environment);
    // JWT
    IApnsSettingsBuilder WithJsonToken(ApnsJsonToken jsonToken);
    IApnsSettingsBuilder SetTopic(string topic);
    // X509 Certificate
    IApnsSettingsBuilder WithCertificate(ApnsCertificate certificate);
    IApnsSettingsBuilder WithX509Certificate2(X509Certificate2 certificate);
    IApnsSettingsBuilder WithPathToX509Certificate2(string pathToCertificate);
    IApnsSettingsBuilder WithPathToX509Certificate2(string pathToCertificate, string password);
    // Create new settings
    ApnsSettings Build();
    //
    IApnsSettingsBuilder Reset();
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

    public IApnsSettingsBuilder SetTopic(string topic)
    {
        _settings.Topic = topic;
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

    public IApnsSettingsBuilder WithPathToX509Certificate2(string pathToCertificate)
    {
        var apnsCertificate = new ApnsCertificate(pathToCertificate);
        _settings.Certificate = apnsCertificate;
        
        return this;
    }
    
    public IApnsSettingsBuilder WithPathToX509Certificate2(string pathToCertificate, string password)
    {
        var apnsCertificate = new ApnsCertificate(pathToCertificate, password);
        _settings.Certificate = apnsCertificate;
        
        return this;
    }

    public ApnsSettings Build()
    {
        new Rule()
            .Check(() => string.IsNullOrEmpty(_settings.Host))
            .OnSuccess(() => throw new EnvironmentNotSetException())
            .Validate();

        new Rule()
            .Check(() => string.IsNullOrEmpty(_settings.Topic))
            .OnSuccess(() => throw new TopicNotSetException())
            .Validate();

        new Rule()
            .Check(() => _settings.IsCertificateAuthorizationBased)
            .Check(() => _settings.IsTokenAuthorizationBased)
            .OnSuccess(() => throw new DuplicatedAuthorizationException())
            .Validate();

        new Rule()
            .Check(() => !_settings.IsCertificateAuthorizationBased)
            .Check(() => !_settings.IsTokenAuthorizationBased)
            .OnSuccess(() => throw new AuthorizationNotSetException())
            .Validate();

        return _settings;
    }

    public IApnsSettingsBuilder Reset()
    {
        _settings = new ApnsSettings();
        return this;
    }
}
