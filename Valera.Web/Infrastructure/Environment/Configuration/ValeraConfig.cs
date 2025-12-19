using ValeraWeb.Domain.Entities;

namespace ValeraWeb.Infrastructure.Environment.Configuration;

public class ValeraConfig
{
    public Health HealthConfig { get; set; } = new Health(0, 0, 0);
    public Mana ManaConfig { get; set; } = new Mana(0, 0, 0);
    public Vitality VitalityConfig { get; set; } = new Vitality(0, 0, 0);
    public Tired TiredConfig { get; set; } = new Tired(0, 0, 0);
    public int Money { get; set; } = 0;


    public ValeraConfig() { }

    /// <summary>
    /// Конфигурация персонажа <see cref="ValeraWeb"/>
    /// </summary>
    /// <param name="configuration"></param>
    public ValeraConfig(IConfigurationSection configuration)
    {
        /// Конфигурация здоровья
        {
            var healthSection = configuration.GetSection(nameof(Health));
            var def = healthSection.GetValue<int>(nameof(Health.Default));
            var min = healthSection.GetValue<int>(nameof(Health.Min));
            var max = healthSection.GetValue<int>(nameof(Health.Max));
            HealthConfig = new Health(def, min, max);
        }
        {
            /// Конфигурация маны
            var manaSection = configuration.GetSection(nameof(Mana));
            var def = manaSection.GetValue<int>(nameof(Mana.Default));
            var min = manaSection.GetValue<int>(nameof(Mana.Min));
            var max = manaSection.GetValue<int>(nameof(Mana.Max));
            ManaConfig = new Mana(def, min, max);
        }
        {
            /// Конфигурация жизнерадостности
            var vitalitySection = configuration.GetSection(nameof(Vitality));
            var def = vitalitySection.GetValue<int>(nameof(Vitality.Default));
            var min = vitalitySection.GetValue<int>(nameof(Vitality.Min));
            var max = vitalitySection.GetValue<int>(nameof(Vitality.Max));
            VitalityConfig = new Vitality(def, min, max);
        }
        {
            /// Конфигурация усталости
            var tiredSection = configuration.GetSection(nameof(Tired));
            var def = tiredSection.GetValue<int>(nameof(Tired.Default));
            var min = tiredSection.GetValue<int>(nameof(Tired.Min));
            var max = tiredSection.GetValue<int>(nameof(Tired.Max));
            TiredConfig = new Tired(def, min, max);
        }

        /// Конфигурация денег
        Money = configuration.GetValue<int>(nameof(Money));
    }

    public void Validate()
    {
        if (HealthConfig.Default < HealthConfig.Min || HealthConfig.Default > HealthConfig.Max)
            throw new ArgumentException("Параметр по умолчанию не в пределах min-max", nameof(HealthConfig));

        if (ManaConfig.Default < ManaConfig.Min || ManaConfig.Default > ManaConfig.Max)
            throw new ArgumentException("Параметр по умолчанию не в пределах min-max", nameof(ManaConfig));

        if (VitalityConfig.Default < VitalityConfig.Min || VitalityConfig.Default > VitalityConfig.Max)
            throw new ArgumentException("Параметр по умолчанию не в пределах min-max", nameof(VitalityConfig));

        if (TiredConfig.Default < TiredConfig.Min || TiredConfig.Default > TiredConfig.Max)
            throw new ArgumentException("Параметр по умолчанию не в пределах min-max", nameof(TiredConfig));

        if (Money > 1_000_000)
            throw new ArgumentException("Ээ, ты не наглей", nameof(Money));
    }

    public sealed record Health(int Default, int Min, int Max) : IMinMaxConfig;
    public sealed record Mana(int Default, int Min, int Max) : IMinMaxConfig;
    public sealed record Vitality(int Default, int Min, int Max) : IMinMaxConfig;
    public sealed record Tired(int Default, int Min, int Max) : IMinMaxConfig;
    public interface IMinMaxConfig
    {
        int Max { get; }
        int Min { get; }
        int Default { get; }
    }
}