namespace ValeraWeb.Infrastructure.Environment.Configuration;

public sealed class JwtOptions
{
    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public string Key { get; init; } = default!;
    public int LifetimeMinutes { get; init; } = 60;
}