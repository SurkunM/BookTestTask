using BooksTestTask.Configuration;
using BooksTestTask.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BooksTestTask.BusinessLogic.Authentication;

public class JwtProvider
{
    private readonly JwtOptions _options;

    private readonly UserManager<UserEntity> _userManager;

    public JwtProvider(IOptions<JwtOptions> options, UserManager<UserEntity> userManager)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    }

    public async Task<string> GenerateTokenAsync(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.UserName!)
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiatesHours));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
