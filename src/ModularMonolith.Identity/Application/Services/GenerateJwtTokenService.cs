using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModularMonolith.Core.WebApi.Options;
using ModularMonolith.Identity.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ModularMonolith.Identity.Application.Services;

internal sealed class GenerateJwtTokenService(IOptions<IdentityOptions> options)
{
    private readonly IdentityOptions _identityOptions = options.Value;

    public string Execute(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        // Adiciona as claims customizadas do usuário
        claims.AddRange(user.Claims.Select(c => new Claim(c.Type, c.Value)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_identityOptions.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _identityOptions.Issuer,
            audience: _identityOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_identityOptions.TokenExpirationInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}