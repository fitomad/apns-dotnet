using System.Security.Cryptography.X509Certificates;
using Fitomad.Apns.Entities;
using Fitomad.Apns.Entities.Settings;

namespace Fitomad.Apns;

public interface IApnsSettingsBuilder
{
    /// <summary>
    /// Set the APNS environment. Could be Development or Production
    /// </summary>
    /// <param name="environment">One of the allowed environments</param>
    /// <returns></returns>
    IApnsSettingsBuilder InEnvironment(ApnsEnvironment environment);
    /// <summary>
    /// The application bundle id.
    /// In case you are going to send Live Activities notifications you don't need to chage the this field, the library appends the proper suffix for Live Activity and VoIp notifications.
    /// </summary>
    /// <param name="topic">The application bundle identifier. For example com.desappstre.Smarty</param>
    /// <returns></returns>
    IApnsSettingsBuilder SetTopic(string topic);
    /// <summary>
    /// Set the JWT Key parameters used to sign the notification requests.
    /// </summary>
    /// <param name="jsonToken">A custom ApnsJsonToken struct with all required details</param>
    /// <returns></returns>
    IApnsSettingsBuilder WithJsonToken(ApnsJsonToken jsonToken);
    /// <summary>
    /// Set the JWT Key parameters used to sign the notification requests
    /// </summary>
    /// <param name="pathToJwt">Path to JWT Key file</param>
    /// <param name="keyId">The JWT Key ID provided by Apple.</param>
    /// <param name="teamId">Your Team Id available in your Apple Developer Program account</param>
    /// <returns></returns>
    IApnsSettingsBuilder WithPathToJsonToken(string pathToJwt, string keyId, string teamId);
    /// <summary>
    /// Set the X509 certificate using to sign notification requests
    /// </summary>
    /// <param name="certificate">The certificate information using a custom ApnsCertificate struct</param>
    /// <returns></returns>
    IApnsSettingsBuilder WithCertificate(ApnsCertificate certificate);
    /// <summary>
    /// A X509Certificate2 certificate
    /// </summary>
    /// <param name="certificate">The .NET class X509Certificate2 that represents your certificate</param>
    /// <returns></returns>
    IApnsSettingsBuilder WithX509Certificate2(X509Certificate2 certificate);
    /// <summary>
    /// Information needed to use the X509 certificate
    /// </summary>
    /// <param name="pathToCertificate">Path to the certificate file</param>
    /// <param name="password">The password used when you export the certificate from your Keychain</param>
    /// <returns></returns>
    IApnsSettingsBuilder WithPathToX509Certificate2(string pathToCertificate, string password);
    /// <summary>
    /// Creates the APNS settings based on your previous method class
    /// </summary>
    /// <returns>The ApnsSettings object</returns>
    ApnsSettings Build();
    /// <summary>
    /// In case you need to restart the builder object
    /// </summary>
    /// <returns></returns>
    IApnsSettingsBuilder Reset();
}