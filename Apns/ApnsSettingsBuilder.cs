using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Settings;
using Fitomad.Apns.Exceptions;
using Fitomad.Apns.Services.Validation;

namespace Fitomad.Apns;

public sealed class ApnsSettingsBuilder: IApnsSettingsBuilder
{
    private ApnsSettings _settings = new();

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

    public IApnsSettingsBuilder WithPathToJsonToken(string pathToJwt, string keyId, string teamId)
    {
        string pattern = @"\-{5}[\w\s]*\-{5}\n(?<key>[\w\s\W]*)\n\-{5}[\w\s]*\-{5}";
        string keyFileContent = File.ReadAllText(pathToJwt);

        var match = Regex.Match(keyFileContent, pattern);
        var key = match.Groups["key"].Value;

        var apnsJsonToken = new ApnsJsonToken
        {
            KeyId = keyId,
            TeamId = teamId,
            Content = key
        };
        
        return WithJsonToken(apnsJsonToken);
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
    
    public IApnsSettingsBuilder WithPathToX509Certificate2(string pathToCertificate, string password)
    {
        var apnsCertificate = new ApnsCertificate(pathToCertificate, password);
        _settings.Certificate = apnsCertificate;
        
        return this;
    }

    public ApnsSettings Build()
    {
        new Rule()
            .VerifyThat(() => string.IsNullOrEmpty(_settings.Host))
            .OnSuccess(() => throw new EnvironmentNotSetException())
            .Validate();

        new Rule()
            .VerifyThat(() => string.IsNullOrEmpty(_settings.Topic))
            .OnSuccess(() => throw new TopicNotSetException())
            .Validate();

        new Rule()
            .VerifyThat(() => _settings.IsCertificateAuthorizationBased)
            .VerifyThat(() => _settings.IsTokenAuthorizationBased)
            .OnSuccess(() => throw new DuplicatedAuthorizationException())
            .Validate();

        new Rule()
            .VerifyThat(() => !_settings.IsCertificateAuthorizationBased)
            .VerifyThat(() => !_settings.IsTokenAuthorizationBased)
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
