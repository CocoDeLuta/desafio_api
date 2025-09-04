using System.Globalization;
using System.Text.Json;

namespace Desafio.Pages.Services;

public static class DataFormattingService
{
    public static void FormatContaData(Dictionary<string, object> item)
    {
        // Formatar pessoa
        if (item.ContainsKey("pessoa") && item["pessoa"] is not null)
        {
            try
            {
                var pessoaElement = (JsonElement)item["pessoa"];
                if (pessoaElement.TryGetProperty("nome", out var nomeElement))
                {
                    item["pessoa"] = nomeElement.GetString() ?? "Nome não encontrado";
                }
                else
                {
                    item["pessoa"] = "Nome não encontrado";
                }
            }
            catch
            {
                item["pessoa"] = "Erro ao processar";
            }
        }

        // Formatar valores monetários
        FormatMonetaryValue(item, "saldo");
        FormatMonetaryValue(item, "limiteSaqueDiario");
        
        // Formatar data
        FormatDate(item, "dataCriacao", "dd/MM/yyyy");
    }

    public static void FormatTransacaoData(Dictionary<string, object> item)
    {
        // Formatar conta
        if (item.ContainsKey("conta") && item["conta"] is not null)
        {
            try
            {
                var contaElement = (JsonElement)item["conta"];
                if (contaElement.TryGetProperty("idConta", out var idContaElement))
                {
                    item["conta"] = idContaElement.GetInt32().ToString();
                }
                else
                {
                    item["conta"] = "ID não encontrado";
                }
            }
            catch
            {
                item["conta"] = "Erro ao processar";
            }
        }

        // Formatar valor
        FormatMonetaryValue(item, "valor");
        
        // Formatar data
        FormatDate(item, "dataTransacao", "dd/MM/yyyy HH:mm");
    }

    public static void FormatPessoaData(Dictionary<string, object> item)
    {
        // Formatar data de nascimento
        FormatDate(item, "dataNascimento", "dd/MM/yyyy");
    }

    private static void FormatMonetaryValue(Dictionary<string, object> item, string key)
    {
        if (!item.ContainsKey(key)) return;

        try
        {
            if (item[key] is JsonElement element && element.ValueKind == JsonValueKind.Number)
            {
                var valor = element.GetDecimal();
                item[key] = valor.ToString("C", new CultureInfo("pt-BR"));
            }
            else if (decimal.TryParse(item[key].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var valor))
            {
                item[key] = valor.ToString("C", new CultureInfo("pt-BR"));
            }
        }
        catch { }
    }

    private static void FormatDate(Dictionary<string, object> item, string key, string format)
    {
        if (!item.ContainsKey(key)) return;

        if (DateTime.TryParse(item[key].ToString(), out var data))
        {
            item[key] = data.ToString(format);
        }
    }
}
