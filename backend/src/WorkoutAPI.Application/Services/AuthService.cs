using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkoutAPI.Application.Abstractions;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Entities;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Infrastructure.Data;
using BCryptNet = BCrypt.Net.BCrypt;

namespace WorkoutAPI.Application.Services;
public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;
    public AuthService(AppDbContext db, IConfiguration config){ _db = db; _config = config; }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var exists = await _db.Users.AnyAsync(u => u.Email == request.Email);
        if (exists) throw new InvalidOperationException("Email already exists.");
        var user = new User
        {
            Email = request.Email.Trim().ToLowerInvariant(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Role = request.Role,
            PasswordHash = BCryptNet.HashPassword(request.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return await CreateTokenAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email.ToLower());
        if (user is null) throw new UnauthorizedAccessException("Invalid credentials.");
        if (!BCryptNet.Verify(request.Password, user.PasswordHash)) throw new UnauthorizedAccessException("Invalid credentials.");
        if (!user.IsActive) throw new UnauthorizedAccessException("User is inactive.");
        return await CreateTokenAsync(user);
    }

    private Task<AuthResponse> CreateTokenAsync(User user)
    {
        var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key missing.");
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var claims = new List<Claim>{
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("fullName", user.FullName)
        };
        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessTokenMinutes"] ?? "120")), signingCredentials: creds);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult(new AuthResponse(jwt, user.Email, user.FullName, user.Role));
    }
}