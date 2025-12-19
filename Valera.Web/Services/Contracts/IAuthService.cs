using ValeraWeb.Integration.ValeraApi.Dto;

namespace ValeraWeb.Services.Contracts;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(UserRegisterRequest req, CancellationToken ct);
    Task<AuthResponse> LoginAsync(UserLoginRequest req, CancellationToken ct);
}
