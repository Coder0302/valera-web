using System.ComponentModel.DataAnnotations;
using ValeraWeb.Infrastructure.Ef.Entities;

namespace ValeraWeb.Infrastructure.Ef.Entities;

public class UserEntity
{
    public Guid Id { get; set; }

    [MaxLength(256)]
    public string Email { get; set; } = default!;

    // Храним безопасно
    public string PasswordHash { get; set; } = default!;
    public string PasswordSalt { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Навигация: один пользователь — много Валер
    public ICollection<ValeraEntity> Valeras { get; set; } = new List<ValeraEntity>();
}
