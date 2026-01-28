using Microsoft.AspNetCore.Mvc;
using PropostaService.Application.Interfaces;
using PropostaService.Domain.Entities;

namespace PropostaService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropostasController : ControllerBase
{
    private readonly IPropostaService _propostaService;

    public PropostasController(IPropostaService propostaService)
    {
        _propostaService = propostaService;
    }

    /// <summary>
    /// Cria uma nova proposta de seguro
    /// </summary>
    /// <param name="request">Dados da proposta</param>
    /// <returns>Proposta criada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PropostaResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CriarProposta([FromBody] CriarPropostaRequest request)
    {
        try
        {
            var proposta = await _propostaService.CriarPropostaAsync(
                request.SeguradoNome,
                request.SeguradoCpf,
                request.ValorPremio
            );

            return CreatedAtAction(
                nameof(ObterProposta),
                new { id = proposta.Id },
                new PropostaResponse(proposta)
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPropostas()
    {
        var propostas = await _propostaService.ListarPropostasAsync();
        var response = propostas.Select(p => new PropostaResponse(p));
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterProposta(Guid id)
    {
        var proposta = await _propostaService.ObterPropostaPorIdAsync(id);
        
        if (proposta == null)
            return NotFound();

        return Ok(new PropostaResponse(proposta));
    }

    /// <summary>
    /// Altera o status da proposta
    /// </summary>
    /// <param name="id">ID da proposta</param>
    /// <param name="request">
    /// Status da proposta:
    /// 1 = EmAnálise
    /// 2 = Aprovada
    /// 3 = Rejeitada
    /// </param>
    [HttpPut("{id}/status")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AlterarStatus(Guid id, [FromBody] AlterarStatusRequest request)
    {
        try
        {
            // Converter número para enum
            if (request.Status < 1 || request.Status > 3)
                return BadRequest(new { message = "Status inválido. Use: 1 = EmAnálise, 2 = Aprovada, 3 = Rejeitada" });
            
            var statusEnum = (StatusProposta)request.Status;
            await _propostaService.AlterarStatusPropostaAsync(id, statusEnum);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}

/// <summary>
/// Request para criar uma proposta
/// </summary>
public record CriarPropostaRequest(string SeguradoNome, string SeguradoCpf, decimal ValorPremio);

/// <summary>
/// Request para alterar status da proposta
/// </summary>
/// <param name="Status">
/// Status da proposta:
/// 1 = EmAnálise
/// 2 = Aprovada
/// 3 = Rejeitada
/// </param>
public record AlterarStatusRequest(
    /// <summary>
    /// Status da proposta: 1 = EmAnálise, 2 = Aprovada, 3 = Rejeitada
    /// </summary>
    int Status
);
public record PropostaResponse(
    Guid Id,
    string SeguradoNome,
    string SeguradoCpf,
    decimal ValorPremio,
    StatusProposta Status,
    DateTime DataCriacao,
    DateTime? DataAtualizacao
)
{
    public PropostaResponse(Proposta proposta) : this(
        proposta.Id,
        proposta.SeguradoNome,
        proposta.SeguradoCpf,
        proposta.ValorPremio,
        proposta.Status,
        proposta.DataCriacao,
        proposta.DataAtualizacao
    ) { }
}
