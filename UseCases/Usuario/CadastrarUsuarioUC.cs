using BibliotecaApi.Application.Abstractions;
using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Usuario.DTO;

namespace BibliotecaApi.UseCases.Usuario;

public class CadastrarUsuarioUC
{
    private readonly UsuarioEntity _usuario = new UsuarioEntity();
    private readonly UsuarioRepository _repository = new UsuarioRepository();
    private readonly IPasswordHasher _passwordHasher;

    public CadastrarUsuarioUC(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public async Task<int> Execute(CadastrarUsuarioInputDTO input)
    {
        if (await _repository.CpfExiste(input.CPF))
            throw new InvalidOperationException("Usuário com este CPF já está cadastrado.");

        string senhaHash = _passwordHasher.Hash(input.Senha);
        _usuario.Cadastrar(input.Nome, input.CPF, input.Email, senhaHash);

        int idNovoUsuario = await _repository.Cadastrar(_usuario);
        return idNovoUsuario;
    }
}