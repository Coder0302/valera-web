using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using project.Services;
using System.Security.Claims;
using System.Text;
using ValeraWeb.Infrastructure.Ef.Database;
using ValeraWeb.Infrastructure.Environment.Configuration;
using ValeraWeb.Integration.Middlewares;
using ValeraWeb.Services;
using ValeraWeb.Services.Contracts;

namespace ValeraWeb;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Регестрируем конфигурацю
        builder.Configuration.AddJsonFile($"appsettings.json");
        builder.Configuration.AddJsonFile($"appsettings.Development.json", true);

        // Создаём и регестрируем AppConfig
        var appConfig = new AppConfig(builder.Configuration.GetSection(nameof(AppConfig)));
        appConfig.Validate();
        builder.Services.AddSingleton(appConfig);
        builder.Services.AddSingleton(appConfig.ValeraConfig);
        builder.Services.AddSingleton<IValeraService, ValeraService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<AppDbContext>();

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = true;
                o.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),

                    RoleClaimType = "role",
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
        builder.Services.AddAuthorization();

        // Регестрируем расширения
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "FrontendCors", policy =>
            {
                policy
                    .WithOrigins(
                        "http://127.0.0.1:5064",
                        "http://localhost:5064",
                        "http://localhost:49557",
                        "http://127.0.0.1:49557"
                    // при нужде добавьте "https://127.0.0.1:5173", "https://localhost:5173"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    // если используете cookie/авторизацию через браузер:
                    // .AllowCredentials()
                    ;
            });
        });
        builder.Services.AddMvc();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(s => s.EnableAnnotations());

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            }
        );

        var app = builder.Build();

        // Проводим миграция базы данных до текущей версии.
        using (var scope = app.Services.CreateAsyncScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        app.UseCors("FrontendCors")
           .UseSwagger()
           .UseSwaggerUI()
           .UseMiddleware(typeof(ExceptionLoggerMiddleware), app.Logger)
           .UseRouting()
           .UseAuthentication()
           .UseAuthorization()
           .UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}