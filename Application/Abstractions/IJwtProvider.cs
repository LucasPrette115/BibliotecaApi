using BibliotecaApi.Domain.Entities;

namespace BibliotecaApi.Application.Abstractions
{
    public interface IJwtProvider
    {
        string GerarToken(UsuarioEntity usuario);
    }
}
