namespace ValeraWeb.Integration.ValeraApi.Dto;

public sealed class UserRegisterRequest
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string Username { get; init; } = default!;
}

public sealed class UserLoginRequest
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}

public sealed class UserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = default!;
}

public sealed class AuthResponse
{
    public string Token { get; init; } = default!;
    public UserDto User { get; init; } = default!;
}