using FluentAssertions;
using PropostaService.Domain.ValueObjects;

namespace PropostaService.Tests.Domain.ValueObjects;

public class CpfTests
{
    [Fact]
    public void Criar_DeveCriarCpfValido_QuandoCpfEhValido()
    {
        // Arrange
        var cpfString = "12345678909";

        // Act
        var cpf = Cpf.Criar(cpfString);

        // Assert
        cpf.Should().NotBeNull();
        cpf.Valor.Should().Be(cpfString);
    }

    [Fact]
    public void Criar_DeveCriarCpfValido_QuandoCpfTemMascara()
    {
        // Arrange
        var cpfComMascara = "123.456.789-09";
        var cpfEsperado = "12345678909";

        // Act
        var cpf = Cpf.Criar(cpfComMascara);

        // Assert
        cpf.Valor.Should().Be(cpfEsperado);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Criar_DeveLancarExcecao_QuandoCpfEhVazioOuNulo(string cpfInvalido)
    {
        // Act
        Action act = () => Cpf.Criar(cpfInvalido);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("CPF não pode ser vazio");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12345")]
    [InlineData("123456789012")]
    public void Criar_DeveLancarExcecao_QuandoCpfNaoTem11Digitos(string cpfInvalido)
    {
        // Act
        Action act = () => Cpf.Criar(cpfInvalido);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("CPF deve conter 11 dígitos");
    }

    [Theory]
    [InlineData("11111111111")]
    [InlineData("00000000000")]
    [InlineData("99999999999")]
    public void Criar_DeveLancarExcecao_QuandoTodosDigitosSaoIguais(string cpfInvalido)
    {
        // Act
        Action act = () => Cpf.Criar(cpfInvalido);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("CPF inválido");
    }

    [Theory]
    [InlineData("12345678901")] // Dígitos verificadores incorretos
    [InlineData("11122233344")] // Dígitos verificadores incorretos
    public void Criar_DeveLancarExcecao_QuandoDigitosVerificadoresSaoInvalidos(string cpfInvalido)
    {
        // Act
        Action act = () => Cpf.Criar(cpfInvalido);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("CPF inválido");
    }

    [Fact]
    public void FormatarComMascara_DeveRetornarCpfFormatado()
    {
        // Arrange
        var cpf = Cpf.Criar("12345678909");

        // Act
        var cpfFormatado = cpf.FormatarComMascara();

        // Assert
        cpfFormatado.Should().Be("123.456.789-09");
    }
}
