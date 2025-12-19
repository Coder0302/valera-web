using Microsoft.AspNetCore.Mvc;
using ValeraWeb.Integration.ValeraApi.Dto;
using ValeraWeb.Services.Contracts;

namespace ValeraWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Register(UserRegisterRequest req, CancellationToken ct)
        => Ok(await auth.RegisterAsync(req, ct));

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Login(UserLoginRequest req, CancellationToken ct)
        => Ok(await auth.LoginAsync(req, ct));
}