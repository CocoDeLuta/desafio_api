using System.Globalization;

namespace Desafio.Pages.Services;

public static class ValidationService
{
    public static string SanitizeCpf(string? cpf)
    {
        var digits = new string((cpf ?? string.Empty).Where(char.IsDigit).ToArray());
        if (digits.Length > 11) digits = digits.Substring(0, 11);
        return digits;
    }

    public static bool TryParseMonetario(string? texto, out decimal valor)
    {
        texto = (texto ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(texto)) { valor = 0m; return false; }
        
        if (decimal.TryParse(texto, NumberStyles.Number, new CultureInfo("pt-BR"), out valor))
        {
            valor = Math.Round(valor, 2, MidpointRounding.AwayFromZero);
            return true;
        }
        
        var alt = texto.Replace(',', '.');
        if (decimal.TryParse(alt, NumberStyles.Number, CultureInfo.InvariantCulture, out valor))
        {
            valor = Math.Round(valor, 2, MidpointRounding.AwayFromZero);
            return true;
        }
        
        return false;
    }

    public static string NormalizeMonetaryInput(string? texto)
    {
        var textoNormalizado = (texto ?? string.Empty).Trim();
        textoNormalizado = textoNormalizado.Replace('.', ',');
        var filtrado = new string(textoNormalizado.Where(c => char.IsDigit(c) || c == ',').ToArray());
        var primeiraVirgula = filtrado.IndexOf(',');
        
        if (primeiraVirgula >= 0)
        {
            var parteInteira = filtrado.Substring(0, primeiraVirgula);
            var parteDecimal = new string(filtrado.Substring(primeiraVirgula + 1).Where(char.IsDigit).ToArray());
            if (parteDecimal.Length > 2) parteDecimal = parteDecimal.Substring(0, 2);
            filtrado = parteInteira + "," + parteDecimal;
        }
        
        return filtrado;
    }

    public static string FormatMonetary(decimal valor)
    {
        return valor.ToString("F2", new CultureInfo("pt-BR"));
    }

    public static bool ValidateCpf(string cpf)
    {
        return !string.IsNullOrWhiteSpace(cpf) && cpf.Length == 11;
    }

    public static bool ValidateRequired(string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public static bool ValidateDateRange(DateTime? date, DateTime minDate, DateTime maxDate)
    {
        return date.HasValue && date.Value >= minDate && date.Value <= maxDate;
    }
}
