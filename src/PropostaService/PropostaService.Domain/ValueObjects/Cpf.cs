namespace PropostaService.Domain.ValueObjects;

public class Cpf
{
    public string Valor { get; private set; }

    private Cpf(string valor)
    {
        Valor = valor;
    }

    public static Cpf Criar(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF não pode ser vazio");

        var cpfLimpo = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpfLimpo.Length != 11)
            throw new ArgumentException("CPF deve conter 11 dígitos");

        if (cpfLimpo.Distinct().Count() == 1)
            throw new ArgumentException("CPF inválido");

        if (!ValidarDigitosVerificadores(cpfLimpo))
            throw new ArgumentException("CPF inválido");

        return new Cpf(cpfLimpo);
    }

    private static bool ValidarDigitosVerificadores(string cpf)
    {
        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += (cpf[i] - '0') * (10 - i);

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (digito1 != (cpf[9] - '0'))
            return false;

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += (cpf[i] - '0') * (11 - i);

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return digito2 == (cpf[10] - '0');
    }

    public override string ToString() => Valor;

    public string FormatarComMascara()
    {
        if (Valor.Length != 11)
            return Valor;

        return $"{Valor.Substring(0, 3)}.{Valor.Substring(3, 3)}.{Valor.Substring(6, 3)}-{Valor.Substring(9, 2)}";
    }
}
