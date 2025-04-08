namespace Apns.Entities;

public enum ApnsEnvironment
{
    Production,
    Development
}

internal static class ApnsEnvironmentExtension
{
    private const string AppleApnsProductionServer = "api.push.apple.com:443";
    private const string AppleApnsDevelopmentServer = "api.sandbox.push.apple.com:443";
    
    internal static string GetApnsServer(this ApnsEnvironment environment)
    {
        var host = environment switch
        {
            ApnsEnvironment.Development => AppleApnsDevelopmentServer,
            ApnsEnvironment.Production => AppleApnsProductionServer,
            _ => AppleApnsDevelopmentServer
        };

        return host;
    }
}
