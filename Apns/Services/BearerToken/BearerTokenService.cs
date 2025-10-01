using Fitomad.Apns.Entities.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Fitomad.Apns.Services.BearerToken;

public class BearerTokenService : IBearerTokenService
{
    private const string BEARER_TOKEN_KEY = "APNS-BEARER-TOKEN";

    private readonly ApnsSettings _settings;

    private readonly IDistributedCache _cache;

    public BearerTokenService(ApnsSettings settings, IDistributedCache cache)
    {
        _settings = settings;
        _cache = cache;
    }

    public string GetBearerToken()
    {
        ApnsJsonToken jsonToken = _settings.JsonToken!;

        string? currBearerToken = _cache.GetString(BEARER_TOKEN_KEY);
        if (currBearerToken is null)
        {
            var privateKey = LoadPrivateKey(jsonToken.Content);
            var securityKey = new ECDsaSecurityKey(privateKey);
            var signCredential = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256);

            // Crear el encabezado del token
            var header = new JwtHeader(signCredential);
            header["kid"] = jsonToken.KeyId;

            // Crear el payload del token
            var payload = new JwtPayload
            {
                { "iss", jsonToken.TeamId },
                { "iat", jsonToken.Timestamp }
            };

            // Genera el token
            var jwt = new JwtSecurityToken(header, payload);
            
            string newBearerToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            _cache.SetString(BEARER_TOKEN_KEY, newBearerToken, new DistributedCacheEntryOptions 
            { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(40) });

            return newBearerToken;
        }
        else
        {
            return currBearerToken;
        }
    }

    private static ECDsa LoadPrivateKey(string privateKey)
    {
        var key = Convert.FromBase64String(privateKey);
        var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(key, out _);
        return ecdsa;
    }
}