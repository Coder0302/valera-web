using ValeraWeb.Infrastructure.Environment.Configuration;
using ValeraWeb.Infrastructure.Ef.Entities;

namespace ValeraWeb.Domain.Entities;

public class Valera(ValeraConfig valeraConfig)
{
    private readonly ValeraConfig _valeraConfig = valeraConfig ?? throw new ArgumentNullException(nameof(valeraConfig));

    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Name { get; set; }

    public int Health { get; set; } = valeraConfig.HealthConfig.Default;
    public int Mana { get; set; } = valeraConfig.ManaConfig.Default;
    public int Vitality { get; set; } = valeraConfig.VitalityConfig.Default;
    public int Tired { get; set; } = valeraConfig.TiredConfig.Default;
    public int Money { get; set; } = valeraConfig.Money;

    public bool TryGoToWork()
    {
        if (Mana >= 50 || Tired >= 10) return false;

        ChangeValues(0, -30, -5, 70, 100);

        return true;
    }

    public void ContemplateNature()
    {
        ChangeValues(0, -10, 1, 10, 0);
    }

    public void DrinkingWineAndWatchingTV()
    {
        ChangeValues(-5, 30, -1, 10, -20);
    }

    public void GoToBar()
    {
        ChangeValues(-10, 60, 1, 40, -100);
    }

    public void DrinkWithBadHumans()
    {
        ChangeValues(-80, 90, 5, 80, -150);
    }

    public void SingingInSubway()
    {
        ChangeValues(0, 10, 1, 20, Mana > 40 && Mana < 70 ? 60 : 10);
    }

    public void Sleep()
    {
        ChangeValues(Mana < 30 ? 90 : 0, -50, Mana > 70 ? -3 : 0, -70, 0);
    }

    private void ChangeValues(int health, int mana, int vitality, int tired, int money)
    {
        Health = GetNewValue(Health + health, _valeraConfig.HealthConfig);
        Mana = GetNewValue(Mana + mana, _valeraConfig.ManaConfig);
        Vitality = GetNewValue(Vitality + vitality, _valeraConfig.VitalityConfig);
        Tired = GetNewValue(Tired + tired, _valeraConfig.TiredConfig);
        Money += money;
    }

    private static int GetNewValue(int newValue, ValeraConfig.IMinMaxConfig minMaxConfig)
        => Math.Max(minMaxConfig.Min, Math.Min(minMaxConfig.Max, newValue));
}