using ContratacaoService.Application.Interfaces;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Repositories;

namespace ContratacaoService.Application.Services;

public class ContratacaoService : IContratacaoService
{
    private readonly IContratacaoRepository _contratacaoRepository;
    private readonly IPropostaServiceClient _propostaServiceClient;

    public ContratacaoService(
        IContratacaoRepository contratacaoRepository,
        IPropostaServiceClient propostaServiceClient)
    {
        _contratacaoRepository = contratacaoRepository;
        _propostaServiceClient = propostaServiceClient;
    }

    public async Task<Contratacao> ContratarPropostaAsync(Guid propostaId)
    {
        var existeContratacao = await _contratacaoRepository.ExistePorPropostaIdAsync(propostaId);
        if (existeContratacao)
            throw new InvalidOperationException($"Já existe uma contratação para a proposta {propostaId}");

        var propostaStatus = await _propostaServiceClient.ObterStatusPropostaAsync(propostaId);
        
        if (propostaStatus == null)
            throw new InvalidOperationException($"Proposta {propostaId} não encontrada");

        var statusNormalizado = propostaStatus.Status.Trim();
        if (statusNormalizado != "Aprovada" && statusNormalizado != "2")
            throw new InvalidOperationException($"A proposta {propostaId} não está aprovada. Status atual: {propostaStatus.Status}");

        var contratacao = new Contratacao(propostaId);
        return await _contratacaoRepository.CriarAsync(contratacao);
    }

    public async Task<IEnumerable<Contratacao>> ListarContratacoesAsync()
    {
        return await _contratacaoRepository.ObterTodasAsync();
    }

    public async Task<Contratacao?> ObterContratacaoPorIdAsync(Guid id)
    {
        return await _contratacaoRepository.ObterPorIdAsync(id);
    }
}
