using BibliotecaApi.Application.Api.Responses;
using BibliotecaApi.Domain.Entities;
using BibliotecaApi.UseCases.Livro;
using BibliotecaApi.UseCases.Livro.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BibliotecaApi.Application.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class LivroController : Controller
{
    private readonly CadastrarLivroUC _cadastrarLivroUC = new CadastrarLivroUC();
    private readonly ListarLivrosUC _listarLivrosUC = new ListarLivrosUC();

    [Authorize]
    [HttpPost]
    [SwaggerOperation(Summary = "Adiciona uma nova categoria retornando o seu respectivo Id")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(ApiResponse<int>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Cadastrar(CadastrarLivroInputDTO input)
    {
        try
        {
            int newId = await _cadastrarLivroUC.Execute(input);
            return Ok(ApiResponse<int>.Ok(newId));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<int>.Falha(ex.Message));
        }

    }

    [HttpGet]
    [SwaggerOperation(Summary = "Retorna a lista completa de livros cadastrados")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<LivroEntity>>))]
    public async Task<IActionResult> Listar()
    {
        try
        {
            var livros = await _listarLivrosUC.Execute();
            return Ok(ApiResponse<IEnumerable<LivroEntity>>.Ok(livros));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<IEnumerable<LivroEntity>>.Falha(ex.Message));
        }
    }
}
