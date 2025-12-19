using System.ComponentModel.DataAnnotations;
using ValeraWeb.Infrastructure.Ef.Entities;

namespace ValeraWeb.Infrastructure.Ef.Entities;

public class UserEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Email { get; set; } = string.Empty;
    public string EmailNormalized { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = default!;

    // "User" | "Admin"
    public string Role { get; set; } = "User";

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
