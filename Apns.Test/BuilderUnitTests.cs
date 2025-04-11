using Apns.Entities;

namespace Apns.Test;

public class BuilderUnitTests
{
    [Fact]
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
    public void TestEnvironmentDuplicatedAuthorization(ApnsEnvironment environment)
    {
        Assert.Throws<DuplicatedAuthorizationException>(() =>
        {
            var token = new ApnsJsonToken(content: "");
            
            new ApnsSettingsBuilder()
                .InEnvironment(environment)
                .SetTopic("fake-topic")
                .WithJsonToken(token)
                .WithPathToX509Certificate2("/Users/adolfo/Documents/Proyectos/Packages/ApnsCertificates/apns-smarty.cer")
                .Build();
        });
    }
    
    [Theory]
    [InlineData(ApnsEnvironment.Development)]
    [InlineData(ApnsEnvironment.Production)]
    public void TestNoTopic(ApnsEnvironment environment)
    {
        Assert.Throws<TopicNotSetException>(() =>
        {
            new ApnsSettingsBuilder()
                .InEnvironment(environment)
                .WithPathToX509Certificate2("/Users/adolfo/Documents/Proyectos/Packages/ApnsCertificates/apns-smarty.cer")
                .Build();
        });
    }
}
