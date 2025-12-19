using ValeraWeb.Integration.ValeraApi.Dto;

namespace ValeraWeb.Services.Contracts;

public interface IValeraService
{
    Task<List<ValeraDto>> GetAll(CancellationToken ct = default);
    Task<ValeraDto> CreateAsync(Guid userId, CreateValeraRequest req, CancellationToken ct = default);

    Task<ValeraDto> TryGoToWorkAsync(Guid id, CancellationToken ct = default);
    Task<ValeraDto> ContemplateNatureAsync(Guid id, CancellationToken ct = default);
    Task<ValeraDto> DrinkingWineAndWatchingTvAsync(Guid id, CancellationToken ct = default);
    Task<ValeraDto> GoToBarAsync(Guid id, CancellationToken ct = default);
    Task<ValeraDto> DrinkWithBadHumansAsync(Guid id, CancellationToken ct = default);
    Task<ValeraDto> SingingInSubwayAsync(Guid id, CancellationToken ct = default);
    Task<ValeraDto> SleepAsync(Guid id, CancellationToken ct = default);
}
