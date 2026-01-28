using PropostaService.Domain.Entities;

namespace PropostaService.Application.Interfaces;

public interface IPropostaService
{
    Task<Proposta> CriarPropostaAsync(string seguradoNome, string seguradoCpf, decimal valorPremio);
    Task<IEnumerable<Proposta>> ListarPropostasAsync();
    Task<Proposta?> ObterPropostaPorIdAsync(Guid id);
    Task AlterarStatusPropostaAsync(Guid id, StatusProposta novoStatus);
}
