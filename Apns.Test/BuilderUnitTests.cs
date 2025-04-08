using Apns;
using Apns.Entities;

namespace Apns.Test;

public class BuilderUnitTests
{
    [Fact]
    public void TestNoEnvironmentNoAuthorization()
    {
        Assert.Throws<EnvironmentNotSetException>(() =>
        {
            var settings = new ApnsSettingsBuilder()
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
            var settings = new ApnsSettingsBuilder()
                .InEnvironment(environment)
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
            
            var settings = new ApnsSettingsBuilder()
                .InEnvironment(environment)
                .WithJsonToken(token)
                .WithPathToX509Certificate2("/fake/path/to/certificate")
                .Build();
        });
        
    }
}
