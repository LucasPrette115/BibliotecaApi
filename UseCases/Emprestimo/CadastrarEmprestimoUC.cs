using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Emprestimo.DTO;

namespace BibliotecaApi.UseCases.Emprestimo;

public class CadastrarEmprestimoUC
{
    private readonly EmprestimoRepository _emprestimoRepository = new EmprestimoRepository();
    private readonly LivroRepository _livroRepository = new LivroRepository();

    public async Task<int> Execute(CadastrarEmprestimoInputDTO input)
    {
        var emprestimo = new EmprestimoEntity();
        emprestimo.Cadastrar(input.IdUsuario, input.IdLivro, input.DataPrevistaDevolucao);

        if (!await _livroRepository.EstaDisponivel(input.IdLivro))
            throw new InvalidOperationException("Este livro já está emprestado e ainda não foi devolvido.");

        int idEmprestimo = await _emprestimoRepository.Cadastrar(emprestimo);

        // Marca o livro como indisponível
        await _livroRepository.MarcarComoIndisponivel(input.IdLivro);

        return idEmprestimo;
    }
}
