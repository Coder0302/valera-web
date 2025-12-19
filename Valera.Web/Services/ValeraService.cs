using Microsoft.EntityFrameworkCore;
using ValeraWeb.Domain.Entities;
using ValeraWeb.Infrastructure.Ef.Database;
using ValeraWeb.Infrastructure.Ef.Entities;
using ValeraWeb.Infrastructure.Ef.Mapping;
using ValeraWeb.Infrastructure.Environment.Configuration;
using ValeraWeb.Integration.ValeraApi.Dto;
using ValeraWeb.Integration.ValeraApi.Mapping;
using ValeraWeb.Services.Contracts;

namespace project.Services;

public sealed class ValeraService(AppDbContext db, ValeraConfig valeraConfig) : IValeraService
{
    public async Task<ValeraDto> CreateAsync(Guid userId, CreateValeraRequest req, CancellationToken ct = default)
    {
        var v = new ValeraWeb.Domain.Entities.Valera(valeraConfig)
        {
            Id = Guid.NewGuid(),
            UserId = userId
        };

        // если переданы начальные значения — применим
        if (req.Health is int h) v.Health = h;
        if (req.Mana is int m) v.Mana = m;
        if (req.Vitality is int vi) v.Vitality = vi;
        if (req.Tired is int ti) v.Tired = ti;
        if (req.Money is int mo) v.Money = mo;

        var e = new ValeraEntity();
        v.ToEntity(e);

        var result = await db.Valeras.AddAsync(e, ct);

        v.FromEntity(result.Entity);

        await db.SaveChangesAsync(ct);

        return v.ToDto(true);
    }

    public async Task<ValeraDto> TryGoToWorkAsync(Guid id, CancellationToken ct = default)
        => await ApplyAsync(id, v => v.TryGoToWork(), ct);

    public async Task<ValeraDto> ContemplateNatureAsync(Guid id, CancellationToken ct = default)
        => await ApplyAsync(id, v => { v.ContemplateNature(); return null; }, ct);

    public async Task<ValeraDto> DrinkingWineAndWatchingTvAsync(Guid id, CancellationToken ct = default)
        => await ApplyAsync(id, v => { v.DrinkingWineAndWatchingTV(); return null; }, ct);

    public async Task<ValeraDto> GoToBarAsync(Guid id, CancellationToken ct = default)
        => await ApplyAsync(id, v => { v.GoToBar(); return null; }, ct);

    public async Task<ValeraDto> DrinkWithBadHumansAsync(Guid id, CancellationToken ct = default)
        => await ApplyAsync(id, v => { v.DrinkWithBadHumans(); return null; }, ct);

    public async Task<ValeraDto> SingingInSubwayAsync(Guid id, CancellationToken ct = default)
        => await ApplyAsync(id, v => { v.SingingInSubway(); return null; }, ct);

    public async Task<ValeraDto> SleepAsync(Guid id, CancellationToken ct = default)
        => await ApplyAsync(id, v => { v.Sleep(); return null; }, ct);

    private async Task<ValeraDto> ApplyAsync(Guid id, Func<ValeraWeb.Domain.Entities.Valera, bool?> action, CancellationToken ct)
    {
        var entity = await db.Valeras
            .FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new KeyNotFoundException($"Valera {id} not found");

        var valera = new ValeraWeb.Domain.Entities.Valera(valeraConfig);
        valera.FromEntity(entity);

        bool? succeeded = action(valera);

        valera.ToEntity(entity);

        await db.SaveChangesAsync(ct);

        return valera.ToDto(succeeded ?? true);
    }

    public async Task<List<ValeraDto>> GetAll(CancellationToken ct = default)
    {
        var entity = await db.Valeras.AsNoTracking().ToListAsync(ct);
        return entity.Select(x => { ValeraWeb.Domain.Entities.Valera v = new(valeraConfig); v.FromEntity(x); return v.ToDto(true); }).ToList();
    }
}
