using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Application.Interfaces;

public interface IContratacaoService
{
    Task<Contratacao> ContratarPropostaAsync(Guid propostaId);
    Task<IEnumerable<Contratacao>> ListarContratacoesAsync();
    Task<Contratacao?> ObterContratacaoPorIdAsync(Guid id);
}
