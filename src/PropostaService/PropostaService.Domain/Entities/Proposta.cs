using PropostaService.Domain.ValueObjects;

namespace PropostaService.Domain.Entities;

public class Proposta
{
    public Guid Id { get; private set; }
    public string SeguradoNome { get; private set; } = string.Empty;
    public string SeguradoCpf { get; private set; } = string.Empty;
    public decimal ValorPremio { get; private set; }
    public StatusProposta Status { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }

    private Proposta() { }
    public Proposta(string seguradoNome, string seguradoCpf, decimal valorPremio)
    {
        if (string.IsNullOrWhiteSpace(seguradoNome))
            throw new ArgumentException("Nome do segurado é obrigatório", nameof(seguradoNome));

        if (seguradoNome.Length < 3 || seguradoNome.Length > 200)
            throw new ArgumentException("Nome deve ter entre 3 e 200 caracteres", nameof(seguradoNome));

        if (valorPremio <= 0)
            throw new ArgumentException("Valor do prêmio deve ser maior que zero", nameof(valorPremio));

        var cpf = Cpf.Criar(seguradoCpf);

        Id = Guid.NewGuid();
        SeguradoNome = seguradoNome.Trim();
        SeguradoCpf = cpf.Valor;
        ValorPremio = valorPremio;
        Status = StatusProposta.EmAnalise;
        DataCriacao = DateTime.UtcNow;
    }

    public void AlterarStatus(StatusProposta novoStatus)
    {
        if (Status == novoStatus)
            return;

        Status = novoStatus;
        DataAtualizacao = DateTime.UtcNow;
    }

    public bool PodeSerContratada()
    {
        return Status == StatusProposta.Aprovada;
    }
}

public enum StatusProposta
{
    EmAnalise = 1,
    Aprovada = 2,
    Rejeitada = 3
}
