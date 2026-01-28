using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ContratacaoService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ContratacaoService.Infrastructure.Clients;

public class PropostaServiceClient : IPropostaServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly ILogger<PropostaServiceClient> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public PropostaServiceClient(HttpClient httpClient, IConfiguration configuration, ILogger<PropostaServiceClient> logger)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["PropostaService:BaseUrl"] ?? "http://localhost:5001";
        _logger = logger;
    }

    public async Task<PropostaStatusResponse?> ObterStatusPropostaAsync(Guid propostaId)
    {
        try
        {
            var url = $"{_baseUrl}/api/propostas/{propostaId}";
            _logger.LogInformation("Buscando proposta {PropostaId} em {Url}", propostaId, url);
            
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Falha ao buscar proposta {PropostaId}. Status: {StatusCode}", propostaId, response.StatusCode);
                return null;
            }

            var proposta = await response.Content.ReadFromJsonAsync<PropostaResponse>(JsonOptions);
            
            if (proposta == null)
            {
                _logger.LogWarning("Resposta vazia ao buscar proposta {PropostaId}", propostaId);
                return null;
            }

            var statusString = proposta.Status.ToString();
            _logger.LogInformation("Proposta {PropostaId} encontrada com status {Status}", propostaId, statusString);
            
            return new PropostaStatusResponse(proposta.Id, statusString);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de comunicação ao buscar proposta {PropostaId}", propostaId);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao buscar proposta {PropostaId}", propostaId);
            return null;
        }
    }

    private record PropostaResponse(
        Guid Id,
        [property: JsonConverter(typeof(JsonStringEnumConverter))] StatusProposta Status
    );
    
    private enum StatusProposta
    {
        EmAnalise = 1,
        Aprovada = 2,
        Rejeitada = 3
    }
}
