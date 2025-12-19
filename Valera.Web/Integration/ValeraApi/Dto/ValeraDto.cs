namespace ValeraWeb.Integration.ValeraApi.Dto;

public record ValeraDto(Guid Id, Guid UserId, int Health, int Mana, int Vitality, int Tired, int Money, bool? Succeeded);

public sealed class CreateValeraRequest
{
    public int? Health { get; init; }
    public int? Mana { get; init; }
    public int? Vitality { get; init; }
    public int? Tired { get; init; }
    public int? Money { get; init; }
}