using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSMessagingApp.Server.Models;
using Microsoft.IdentityModel.Tokens;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public AuthService(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var agent = await _context.Agents.FirstOrDefaultAsync(a => a.Username == username);
        if (agent == null || !VerifyPassword(password, agent.PasswordHash))
            return null;

        var token = GenerateJwtToken(agent);
        return token;
    }

    private string GenerateJwtToken(Agent agent)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, agent.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, agent.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        // Implement password verification logic (e.g., using BCrypt)
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
