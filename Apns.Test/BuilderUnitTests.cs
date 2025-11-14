using Microsoft.Extensions.Configuration;
using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Settings;
using Fitomad.Apns.Exceptions;

namespace Fitomad.Apns.Test;

public class BuilderUnitTests
{
    private readonly string _certificatePath;
    private readonly string _certificatePassword;
    
    public BuilderUnitTests()
    {
        var userSecretsConfiguration = new ConfigurationBuilder()
            .AddUserSecrets<BuilderUnitTests>()
            .Build();

        _certificatePath = userSecretsConfiguration.GetValue<string>("Apns:CertPath");
        _certificatePassword = userSecretsConfiguration.GetValue<string>("Apns:CertPassword");

    }
    [Fact]
    [Trait("CI", "FALSE")]
    public void TestNoEnvironmentNoAuthorization()
    {
        Assert.Throws<EnvironmentNotSetException>(() =>
        {
            new ApnsSettingsBuilder()
                .SetTopic("fake-topic")
                .Build();
        });
    }
    
    [Theory]
    [InlineData(ApnsEnvironment.Development)]
    [InlineData(ApnsEnvironment.Production)]
    [Trait("CI", "FALSE")]
    public void TestEnvironmentNoAuthorization(ApnsEnvironment environment)
    {
        Assert.Throws<AuthorizationNotSetException>(() =>
        {
            new ApnsSettingsBuilder()
                .InEnvironment(environment)
                .SetTopic("fake-topic")
                .Build();
        });
    }
    
    [Theory]
    [InlineData(ApnsEnvironment.Development)]
    [InlineData(ApnsEnvironment.Production)]
    [Trait("CI", "FALSE")]
    public void TestEnvironmentDuplicatedAuthorization(ApnsEnvironment environment)
    {
        Assert.Throws<DuplicatedAuthorizationException>(() =>
        {
            var token = new ApnsJsonToken();
            
            new ApnsSettingsBuilder()
                .InEnvironment(environment)
                .SetTopic("fake-topic")
                .WithJsonToken(token)
                .WithPathToX509Certificate2(_certificatePath, _certificatePassword)
                .Build();
        });
    }
    
    [Theory]
    [InlineData(ApnsEnvironment.Development)]
    [InlineData(ApnsEnvironment.Production)]
    [Trait("CI", "FALSE")]
    public void TestNoTopic(ApnsEnvironment environment)
    {
        Assert.Throws<TopicNotSetException>(() =>
        {
            new ApnsSettingsBuilder()
                .InEnvironment(environment)
                .WithPathToX509Certificate2(_certificatePath, _certificatePassword)
                .Build();
        });
    }
}
