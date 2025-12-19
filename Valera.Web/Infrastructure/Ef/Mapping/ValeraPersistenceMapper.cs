using ValeraWeb.Domain.Entities;
using ValeraWeb.Infrastructure.Ef.Entities;

namespace ValeraWeb.Infrastructure.Ef.Mapping;

public static class ValeraPersistenceMapper
{
    public static void ToEntity(this Domain.Entities.Valera valera, ValeraEntity entity)
    {
        entity.Id = valera.Id;
        entity.Health = valera.Health;
        entity.Mana = valera.Mana;
        entity.Money = valera.Money;
        entity.Tired = valera.Tired;
        entity.Vitality = valera.Vitality;
        entity.UserId = valera.UserId;
    }

    public static void FromEntity(this Domain.Entities.Valera valera, ValeraEntity entity)
    {
        valera.Id = entity.Id;
        valera.Health = entity.Health;
        valera.Mana = entity.Mana;
        valera.Money = entity.Money;
        valera.Tired = entity.Tired;
        valera.Vitality = entity.Vitality;
        valera.UserId = entity.UserId;
    }
}
