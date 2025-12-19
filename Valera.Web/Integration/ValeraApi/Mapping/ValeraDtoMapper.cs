using ValeraWeb.Integration.ValeraApi.Dto;

namespace ValeraWeb.Integration.ValeraApi.Mapping;

public static class ValeraDtoMapper
{
    public static ValeraDto ToDto(this Domain.Entities.Valera v, bool isSucceeded) =>
        new(v.Id, v.UserId, v.Health, v.Mana, v.Vitality, v.Tired, v.Money, isSucceeded);
}
