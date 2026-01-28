using PropostaService.Domain.Entities;

namespace PropostaService.Domain.Repositories;

public interface IPropostaRepository
{
    Task<Proposta?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Proposta>> ObterTodasAsync();
    Task<Proposta> CriarAsync(Proposta proposta);
    Task AtualizarAsync(Proposta proposta);
    Task<bool> ExisteAsync(Guid id);
    Task<bool> ExistePorCpfAsync(string cpf);
}
