namespace ContratacaoService.Application.Interfaces;

public interface IPropostaServiceClient
{
    Task<PropostaStatusResponse?> ObterStatusPropostaAsync(Guid propostaId);
}

public record PropostaStatusResponse(Guid Id, string Status);
