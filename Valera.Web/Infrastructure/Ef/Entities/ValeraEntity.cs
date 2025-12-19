using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValeraWeb.Infrastructure.Ef.Entities;

namespace ValeraWeb.Infrastructure.Ef.Entities;

public class ValeraEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public int Health { get; set; }

    [Required]
    public int Mana { get; set; }

    [Required]
    public int Vitality { get; set; }

    [Required]
    public int Tired { get; set; }

    [Required]
    public int Money { get; set; }


    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}
