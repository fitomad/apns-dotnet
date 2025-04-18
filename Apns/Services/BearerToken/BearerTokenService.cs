using System;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Fitomad.Apns.Entities.Settings;

namespace Fitomad.Apns.Services.BearerToken;

public class BearerTokenService: IBearerTokenService
{
    public string MakeBearerToken(ApnsJsonToken jsonToken)
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
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    private static ECDsa LoadPrivateKey(string privateKey)
    {
        var key = Convert.FromBase64String(privateKey);
        var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(key, out _);
        return ecdsa;
    }
}