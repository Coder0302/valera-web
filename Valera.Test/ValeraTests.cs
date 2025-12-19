using Moq;
using ValeraWeb.Domain.Entities;
using ValeraWeb.Infrastructure.Environment.Configuration;

namespace ValeraTest.Tests;

public class ValeraTests
{
    private ValeraConfig BuildConfig(
        (int min, int max, int def)? health = null,
        (int min, int max, int def)? mana = null,
        (int min, int max, int def)? vitality = null,
        (int min, int max, int def)? tired = null,
        int? money = null)
    {
        var h = health ?? (0, 100, 50);
        var m = mana ?? (0, 100, 20);
        var v = vitality ?? (0, 100, 10);
        var t = tired ?? (0, 100, 0);

        return new ValeraConfig
        {
            HealthConfig = new ValeraConfig.Health(h.def, h.min, h.max),
            ManaConfig = new ValeraConfig.Mana(m.def, m.min, m.max),
            VitalityConfig = new ValeraConfig.Vitality(v.def, v.min, v.max),
            TiredConfig = new ValeraConfig.Tired(t.def, t.min, t.max),
            Money = money ?? 200
        };
    }

    private ValeraWeb.Domain.Entities.Valera CreateSubject(ValeraConfig cfg) => new(cfg);

    [Fact]
    public void Ctor_NullConfig_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ValeraWeb.Domain.Entities.Valera(null!));
    }

    [Fact]
    public void Ctor_Initializes_From_Config_Defaults()
    {
        var cfg = BuildConfig(
            health: (0, 100, 55),
            mana: (0, 100, 33),
            vitality: (0, 100, 7),
            tired: (0, 100, 2),
            money: 123);

        var sut = CreateSubject(cfg);

        Assert.Equal(55, sut.Health);
        Assert.Equal(33, sut.Mana);
        Assert.Equal(7, sut.Vitality);
        Assert.Equal(2, sut.Tired);
        Assert.Equal(123, sut.Money);
    }

    [Fact]
    public void TryGoToWork_Fails_When_Mana_GE_50()
    {
        var cfg = BuildConfig(mana: (0, 100, 50), tired: (0, 100, 0));
        var sut = CreateSubject(cfg);

        var ok = sut.TryGoToWork();

        Assert.False(ok);
        Assert.Equal(50, sut.Mana);
        Assert.Equal(0, sut.Tired);
    }

    [Fact]
    public void TryGoToWork_Fails_When_Tired_GE_10()
    {
        var cfg = BuildConfig(mana: (0, 100, 10), tired: (0, 100, 10));
        var sut = CreateSubject(cfg);

        var ok = sut.TryGoToWork();

        Assert.False(ok);
        Assert.Equal(10, sut.Tired);
    }

    [Fact]
    public void TryGoToWork_Succeeds_And_Applies_Effects_With_Clamping()
    {
        var cfg = BuildConfig(
            health: (0, 100, 95),
            mana: (0, 100, 20),
            vitality: (0, 100, 3),
            tired: (0, 100, 5),
            money: 100);
        var sut = CreateSubject(cfg);

        var ok = sut.TryGoToWork();

        Assert.True(ok);
        Assert.Equal(95, sut.Health);
        Assert.Equal(0, sut.Mana);
        Assert.Equal(0, sut.Vitality);
        Assert.Equal(75, sut.Tired);
        Assert.Equal(200, sut.Money);
    }

    [Fact]
    public void ContemplateNature_Applies_Expected_Changes()
    {
        var cfg = BuildConfig(health: (0, 100, 40), mana: (0, 100, 20), vitality: (0, 100, 10), tired: (0, 100, 0), money: 0);
        var sut = CreateSubject(cfg);

        sut.ContemplateNature();

        Assert.Equal(40, sut.Health);
        Assert.Equal(10, sut.Mana);
        Assert.Equal(11, sut.Vitality);
        Assert.Equal(10, sut.Tired);
        Assert.Equal(0, sut.Money);
    }

    [Fact]
    public void DrinkingWineAndWatchingTV_Applies_Expected_Changes_With_Clamping()
    {
        var cfg = BuildConfig(health: (0, 100, 3), mana: (0, 100, 80), vitality: (0, 100, 0), tired: (0, 100, 95), money: 50);
        var sut = CreateSubject(cfg);

        sut.DrinkingWineAndWatchingTV();

        Assert.Equal(0, sut.Health);
        Assert.Equal(100, sut.Mana);
        Assert.Equal(0, sut.Vitality);
        Assert.Equal(100, sut.Tired);
        Assert.Equal(30, sut.Money);
    }

    [Fact]
    public void GoToBar_Applies_Expected_Changes()
    {
        var cfg = BuildConfig(health: (0, 100, 50), mana: (0, 100, 0), vitality: (0, 100, 10), tired: (0, 100, 0), money: 150);
        var sut = CreateSubject(cfg);

        sut.GoToBar();

        Assert.Equal(40, sut.Health);
        Assert.Equal(60, sut.Mana);
        Assert.Equal(11, sut.Vitality);
        Assert.Equal(40, sut.Tired);
        Assert.Equal(50, sut.Money);
    }

    [Fact]
    public void DrinkWithBadHumans_Applies_Expected_Changes_With_Clamping()
    {
        var cfg = BuildConfig(health: (0, 100, 70), mana: (0, 100, 20), vitality: (0, 100, 98), tired: (0, 100, 30), money: 200);
        var sut = CreateSubject(cfg);

        sut.DrinkWithBadHumans();

        Assert.Equal(0, sut.Health);
        Assert.Equal(100, sut.Mana);
        Assert.Equal(100, sut.Vitality);
        Assert.Equal(100, sut.Tired);
        Assert.Equal(50, sut.Money);
    }

    [Fact]
    public void SingingInSubway_Money_Bonus_Dependent_On_Mana_Low_Or_High()
    {
        var cfg = BuildConfig(mana: (0, 100, 40), money: 10);
        var sut = CreateSubject(cfg);

        sut.SingingInSubway();

        Assert.Equal(50, sut.Mana);
        Assert.Equal(11, sut.Vitality);
        Assert.Equal(20, sut.Tired);
        Assert.Equal(20, sut.Money); // +10 (без бонуса)
    }

    [Fact]
    public void SingingInSubway_Money_Bonus_When_Mana_Between_41_And_69()
    {
        var cfg = BuildConfig(mana: (0, 100, 65), money: 0);
        var sut = CreateSubject(cfg);

        sut.SingingInSubway();

        Assert.Equal(75, sut.Mana);
        Assert.Equal(60, sut.Money); // бонус
    }

    [Fact]
    public void SingingInSubway_No_Bonus_At_Upper_Boundary_70()
    {
        var cfg = BuildConfig(mana: (0, 100, 70), money: 5);
        var sut = CreateSubject(cfg);

        sut.SingingInSubway();

        Assert.Equal(15, sut.Money); // +10, т.к. 70 не < 70
    }

    [Fact]
    public void Sleep_Branch_When_Mana_Less_Than_30_Increases_Health()
    {
        var cfg = BuildConfig(health: (0, 100, 95), mana: (0, 100, 20), vitality: (0, 100, 50), tired: (0, 100, 80), money: 0);
        var sut = CreateSubject(cfg);

        sut.Sleep();

        Assert.Equal(100, sut.Health);
        Assert.Equal(0, sut.Mana);
        Assert.Equal(50, sut.Vitality);
        Assert.Equal(10, sut.Tired);
    }

    [Fact]
    public void Sleep_Branch_When_Mana_Greater_Than_70_Reduces_Vitality()
    {
        var cfg = BuildConfig(health: (0, 100, 60), mana: (0, 100, 80), vitality: (0, 100, 2), tired: (0, 100, 50));
        var sut = CreateSubject(cfg);

        sut.Sleep();

        Assert.Equal(60, sut.Health);
        Assert.Equal(30, sut.Mana);
        Assert.Equal(0, sut.Vitality);
        Assert.Equal(0, sut.Tired);
    }

    [Fact]
    public void Values_Are_Clamped_To_Config_MinMax()
    {
        var cfg = BuildConfig(
            health: (10, 90, 85),
            mana: (5, 60, 6),
            vitality: (0, 10, 9),
            tired: (0, 20, 19),
            money: 0);
        var sut = CreateSubject(cfg);

        sut.ContemplateNature();
        sut.DrinkWithBadHumans();

        Assert.InRange(sut.Health, 10, 90);
        Assert.InRange(sut.Mana, 5, 60);
        Assert.InRange(sut.Vitality, 0, 10);
        Assert.InRange(sut.Tired, 0, 20);
        // Money — без клампа
    }
}
