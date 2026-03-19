using BibliotecaApi.Application.Abstractions;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Usuario.DTO;

namespace BibliotecaApi.UseCases.Usuario
{
    public class LoginUsuarioUC
    {
        private readonly UsuarioRepository _repository = new();
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public LoginUsuarioUC(IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> Execute(LoginUsuarioInputDTO input)
        {
            var usuario = await _repository.ObterPorEmail(input.Email);
            if (usuario == null || !_passwordHasher.Verificar(input.Senha, usuario.SenhaHash!))
                throw new InvalidOperationException("Email ou senha inválidos.");

            return _jwtProvider.GerarToken(usuario);
        }
    }
}
