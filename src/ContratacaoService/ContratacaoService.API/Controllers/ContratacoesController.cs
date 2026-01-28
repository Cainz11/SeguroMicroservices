using Microsoft.AspNetCore.Mvc;
using ContratacaoService.Application.Interfaces;

namespace ContratacaoService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContratacoesController : ControllerBase
{
    private readonly IContratacaoService _contratacaoService;

    public ContratacoesController(IContratacaoService contratacaoService)
    {
        _contratacaoService = contratacaoService;
    }

    [HttpPost]
    public async Task<IActionResult> ContratarProposta([FromBody] ContratarPropostaRequest request)
    {
        try
        {
            var contratacao = await _contratacaoService.ContratarPropostaAsync(request.PropostaId);
            return CreatedAtAction(
                nameof(ObterContratacao),
                new { id = contratacao.Id },
                new ContratacaoResponse(contratacao)
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarContratacoes()
    {
        var contratacoes = await _contratacaoService.ListarContratacoesAsync();
        var response = contratacoes.Select(c => new ContratacaoResponse(c));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterContratacao(Guid id)
    {
        var contratacao = await _contratacaoService.ObterContratacaoPorIdAsync(id);
        
        if (contratacao == null)
            return NotFound();

        return Ok(new ContratacaoResponse(contratacao));
    }
}

public record ContratarPropostaRequest(Guid PropostaId);
public record ContratacaoResponse(
    Guid Id,
    Guid PropostaId,
    DateTime DataContratacao
)
{
    public ContratacaoResponse(Domain.Entities.Contratacao contratacao) : this(
        contratacao.Id,
        contratacao.PropostaId,
        contratacao.DataContratacao
    ) { }
}
