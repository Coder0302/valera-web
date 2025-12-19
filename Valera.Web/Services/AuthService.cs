using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ValeraWeb.Infrastructure.Ef.Database;
using ValeraWeb.Infrastructure.Ef.Entities;
using ValeraWeb.Infrastructure.Environment.Configuration;
using ValeraWeb.Integration.ValeraApi.Dto;
using ValeraWeb.Security;
using ValeraWeb.Services.Contracts;

namespace ValeraWeb.Services;

public sealed class AuthService(AppDbContext db, IOptions<JwtOptions> jwtOptions) : IAuthService
{
    private readonly JwtOptions _jwt = jwtOptions.Value;

    public async Task<AuthResponse> RegisterAsync(UserRegisterRequest req, CancellationToken ct)
    {
        var email = req.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(u => u.Email == email, ct))
            throw new InvalidOperationException("User with this email already exists.");

        var (hash, salt) = PasswordHasher.HashPassword(req.Password);
        var user = new UserEntity { Id = Guid.NewGuid(), Email = email, PasswordHash = hash, PasswordSalt = salt };
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        return new AuthResponse
        {
            Token = JwtFactory.IssueToken(user.Id, user.Email, _jwt),
            User = new UserDto { Id = user.Id, Email = user.Email }
        };
    }

    public async Task<AuthResponse> LoginAsync(UserLoginRequest req, CancellationToken ct)
    {
        var email = req.Email.Trim().ToLowerInvariant();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email, ct)
                   ?? throw new InvalidOperationException("Invalid credentials.");

        if (!PasswordHasher.Verify(req.Password, user.PasswordHash, user.PasswordSalt))
            throw new InvalidOperationException("Invalid credentials.");

        return new AuthResponse
        {
            Token = JwtFactory.IssueToken(user.Id, user.Email, _jwt),
            User = new UserDto { Id = user.Id, Email = user.Email }
        };
    }
}