
using BibliotecaApi.Application.Abstractions;
using BibliotecaApi.Infrastructure.Authentication;
using BibliotecaApi.UseCases.Usuario;

namespace BibliotecaApi.Infrastructure.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddUseCases();
        return services;
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<LoginUsuarioUC>();
        services.AddScoped<CadastrarUsuarioUC>();
        return services;
    }
}
