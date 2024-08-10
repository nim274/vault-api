using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Vault.API.Authentication;

public interface IUserService
{
    Task<string> GetAuthenticationToken(string username, string password);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    public async Task<string> GetAuthenticationToken(string username, string password)
    {
        var user = await _userRepository.GetUser(username, password);

        if (user == null)
            return null;


        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"]!, _configuration["Jwt:Audience"], claims, DateTime.Now, DateTime.Now.AddDays(5), credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
