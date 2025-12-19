using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValeraWeb.Integration.ValeraApi.Dto;
using ValeraWeb.Services.Contracts;
using ValeraWeb.Security;

namespace ValeraWeb.Web.Controllers;

// URL базовый: /api/valera
[Authorize]
[ApiController]
[Route("Api/[controller]")]
public sealed class ValeraController(IValeraService service) : ControllerBase
{
    private readonly IValeraService _service = service;

    // GET /api/valera
    [HttpGet("")]
    [ProducesResponseType(typeof(ValeraDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ValeraDto>>> GetAll(CancellationToken ct)
    {
        var dto = await _service.GetAll(ct);
        if (!User.IsAdmin())
        {
            var userId = User.GetUserId();
            dto = [.. dto.Where(x => x.UserId == userId)];
        }
        return Ok(dto);
    }

    // GET /api/valera
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ValeraDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ValeraDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var dto = (await _service.GetAll(ct)).FirstOrDefault(x=>x.Id == id);
        return Ok(dto);
    }

    // POST /api/valera/
    [HttpPost]
    [ProducesResponseType(typeof(ValeraDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ValeraDto>> Create([FromBody] CreateValeraRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var dto = await service.CreateAsync(userId, req, ct);
        return Ok(dto);
    }

    // POST /api/valera/{id}/work
    [HttpPost("{id:guid}/work")]
    [ProducesResponseType(typeof(ValeraDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ValeraDto>> TryGoToWork(Guid id, CancellationToken ct)
    {
        if (User.GetUserId() != id && !User.IsAdmin()) return Forbid();

        var dto = await _service.TryGoToWorkAsync(id, ct);
        return Ok(dto);
    }

    // POST /api/valera/{id}/contemplate
    [HttpPost("{id:guid}/contemplate")]
    [ProducesResponseType(typeof(ValeraDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ValeraDto>> ContemplateNature(Guid id, CancellationToken ct)
    {
        if (User.GetUserId() != id && !User.IsAdmin()) return Forbid();

        var dto = await _service.ContemplateNatureAsync(id, ct);
        return Ok(dto);
    }

    // POST /api/valera/{id}/wine-tv
    [HttpPost("{id:guid}/wine-tv")]
    [ProducesResponseType(typeof(ValeraDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ValeraDto>> DrinkingWineAndWatchingTv(Guid id, CancellationToken ct)
    {
        if (User.GetUserId() != id && !User.IsAdmin()) return Forbid();

        var dto = await _service.DrinkingWineAndWatchingTvAsync(id, ct);
        return Ok(dto);
    }

    // POST /api/valera/{id}/bar
    [HttpPost("{id:guid}/bar")]
    [ProducesResponseType(typeof(ValeraDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ValeraDto>> GoToBar(Guid id, CancellationToken ct)
    {
        if (User.GetUserId() != id && !User.IsAdmin()) return Forbid();

        var dto = await _service.GoToBarAsync(id, ct);
        return Ok(dto);
    }

    // POST /api/valera/{id}/bad-company
    [HttpPost("{id:guid}/bad-company")]
    [ProducesResponseType(typeof(ValeraDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ValeraDto>> DrinkWithBadHumans(Guid id, CancellationToken ct)
    {
        if (User.GetUserId() != id && !User.IsAdmin()) return Forbid();

        var dto = await _service.DrinkWithBadHumansAsync(id, ct);
        return Ok(dto);
    }

    // POST /api/valera/{id}/subway-sing
    [HttpPost("{id:guid}/subway-sing")]
    [ProducesResponseType(typeof(ValeraDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ValeraDto>> SingingInSubway(Guid id, CancellationToken ct)
    {
        if (User.GetUserId() != id && !User.IsAdmin()) return Forbid();

        var dto = await _service.SingingInSubwayAsync(id, ct);
        return Ok(dto);
    }

    // POST /api/valera/{id}/sleep
    [HttpPost("{id:guid}/sleep")]
    [ProducesResponseType(typeof(ValeraDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ValeraDto>> Sleep(Guid id, CancellationToken ct)
    {
        if (User.GetUserId() != id && !User.IsAdmin()) return Forbid();

        var dto = await _service.SleepAsync(id, ct);
        return Ok(dto);
    }
}