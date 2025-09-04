using System;
using System.Text.RegularExpressions;

namespace Desafio.Services.Helpers;

public class ValidarCpf
{
    public static bool Validar(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        cpf = Regex.Replace(cpf, "[^0-9]", ""); // Remove caracteres não numéricos

        // Verifica se tem exatamente 11 caracteres
        if (cpf.Length != 11) return false;

        // Verifica se todos os caracteres são numéricos so para garantir
        return long.TryParse(cpf, out _);
    }

}
