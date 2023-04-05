using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Trivia.Auth.Options;
using Trivia.Auth.Services;

namespace Trivia.Auth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<DatabaseOptions>(
            builder.Configuration.GetSection("Database")
        );
        builder.Services.Configure<SecurityOptions>(
            builder.Configuration.GetSection("Security")
        );
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}