using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using graph.chat.server.abstractions;
using graph.chat.server.abstractions.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace graph.chat.server.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthenticationService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<Token> AuthenticateAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
            throw new Exception("User not found");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded) throw new Exception("Auth failed");
        
        return CreateToken(user);
    }
    
    public async Task<Token> RegisterAsync(string username, string password)
    {
        var user = new IdentityUser
        {
            UserName = username,
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new Exception("Register failed");

        return CreateToken(user);
    }
    
    private static Token CreateToken(IdentityUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, "User")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("super@secret@secret"));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            "localhost",
            "localhost",
            claims,
            expires: DateTime.UtcNow.AddMinutes(300),
            signingCredentials: credentials
        );

        return new Token
        {
            Value = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        };
    }
}