using PropostaService.Application.Interfaces;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Repositories;
using PropostaService.Domain.ValueObjects;

namespace PropostaService.Application.Services;

public class PropostaService : IPropostaService
{
    private readonly IPropostaRepository _propostaRepository;

    public PropostaService(IPropostaRepository propostaRepository)
    {
        _propostaRepository = propostaRepository;
    }

    public async Task<Proposta> CriarPropostaAsync(string seguradoNome, string seguradoCpf, decimal valorPremio)
    {
        var cpf = Cpf.Criar(seguradoCpf);
        
        var existeProposta = await _propostaRepository.ExistePorCpfAsync(cpf.Valor);
        if (existeProposta)
            throw new InvalidOperationException($"Já existe uma proposta cadastrada para o CPF {cpf.Valor}");
        
        var proposta = new Proposta(seguradoNome, seguradoCpf, valorPremio);
        return await _propostaRepository.CriarAsync(proposta);
    }

    public async Task<IEnumerable<Proposta>> ListarPropostasAsync()
    {
        return await _propostaRepository.ObterTodasAsync();
    }

    public async Task<Proposta?> ObterPropostaPorIdAsync(Guid id)
    {
        return await _propostaRepository.ObterPorIdAsync(id);
    }

    public async Task AlterarStatusPropostaAsync(Guid id, StatusProposta novoStatus)
    {
        var proposta = await _propostaRepository.ObterPorIdAsync(id);
        
        if (proposta == null)
            throw new InvalidOperationException($"Proposta com ID {id} não encontrada");

        proposta.AlterarStatus(novoStatus);
        await _propostaRepository.AtualizarAsync(proposta);
    }
}
