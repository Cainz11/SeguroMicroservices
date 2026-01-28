using FluentAssertions;
using PropostaService.Domain.Entities;

namespace PropostaService.Tests.Domain.Entities;

public class PropostaTests
{
    [Fact]
    public void Criar_DeveCriarPropostaComStatusEmAnalise()
    {
        // Arrange
        var nome = "João Silva";
        var cpf = "12345678909";
        var valor = 1500m;

        // Act
        var proposta = new Proposta(nome, cpf, valor);

        // Assert
        proposta.Should().NotBeNull();
        proposta.Id.Should().NotBeEmpty();
        proposta.SeguradoNome.Should().Be(nome);
        proposta.SeguradoCpf.Should().Be(cpf);
        proposta.ValorPremio.Should().Be(valor);
        proposta.Status.Should().Be(StatusProposta.EmAnalise);
        proposta.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Criar_DeveLancarExcecao_QuandoNomeEhInvalido(string nomeInvalido)
    {
        // Arrange
        var cpf = "12345678909";
        var valor = 1500m;

        // Act
        Action act = () => new Proposta(nomeInvalido, cpf, valor);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Nome do segurado é obrigatório*");
    }

    [Theory]
    [InlineData("AB")]
    [InlineData("X")]
    public void Criar_DeveLancarExcecao_QuandoNomeEhMuitoCurto(string nomeCurto)
    {
        // Arrange
        var cpf = "12345678909";
        var valor = 1500m;

        // Act
        Action act = () => new Proposta(nomeCurto, cpf, valor);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Nome deve ter entre 3 e 200 caracteres*");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Criar_DeveLancarExcecao_QuandoValorEhInvalido(decimal valorInvalido)
    {
        // Arrange
        var nome = "João Silva";
        var cpf = "12345678909";

        // Act
        Action act = () => new Proposta(nome, cpf, valorInvalido);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Valor do prêmio deve ser maior que zero*");
    }

    [Fact]
    public void AlterarStatus_DeveAlterarStatusEAtualizarDataAtualizacao()
    {
        // Arrange
        var proposta = new Proposta("João Silva", "12345678909", 1500m);
        var dataAntes = proposta.DataAtualizacao;

        // Act
        proposta.AlterarStatus(StatusProposta.Aprovada);

        // Assert
        proposta.Status.Should().Be(StatusProposta.Aprovada);
        proposta.DataAtualizacao.Should().NotBeNull();
        proposta.DataAtualizacao.Should().BeAfter(dataAntes ?? DateTime.MinValue);
    }

    [Fact]
    public void AlterarStatus_NaoDeveAtualizarData_QuandoStatusEhOMesmo()
    {
        // Arrange
        var proposta = new Proposta("João Silva", "12345678909", 1500m);
        var dataAntes = proposta.DataAtualizacao;

        // Act
        proposta.AlterarStatus(StatusProposta.EmAnalise);

        // Assert
        proposta.DataAtualizacao.Should().Be(dataAntes);
    }

    [Fact]
    public void PodeSerContratada_DeveRetornarTrue_QuandoStatusEhAprovada()
    {
        // Arrange
        var proposta = new Proposta("João Silva", "12345678909", 1500m);
        proposta.AlterarStatus(StatusProposta.Aprovada);

        // Act
        var podeContratar = proposta.PodeSerContratada();

        // Assert
        podeContratar.Should().BeTrue();
    }

    [Theory]
    [InlineData(StatusProposta.EmAnalise)]
    [InlineData(StatusProposta.Rejeitada)]
    public void PodeSerContratada_DeveRetornarFalse_QuandoStatusNaoEhAprovada(StatusProposta status)
    {
        // Arrange
        var proposta = new Proposta("João Silva", "12345678909", 1500m);
        proposta.AlterarStatus(status);

        // Act
        var podeContratar = proposta.PodeSerContratada();

        // Assert
        podeContratar.Should().BeFalse();
    }
}
