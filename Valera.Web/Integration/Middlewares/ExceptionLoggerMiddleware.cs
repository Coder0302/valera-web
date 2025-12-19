using Microsoft.AspNetCore.Mvc;

namespace ValeraWeb.Integration.Middlewares;

public class ExceptionLoggerMiddleware(RequestDelegate next, ILogger logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Ошибка в параметрах");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Некорректный запрос",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, "Объект не найден");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Несуществующий Id",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка на сервере");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Внутренняя ошибка сервера",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }
}
