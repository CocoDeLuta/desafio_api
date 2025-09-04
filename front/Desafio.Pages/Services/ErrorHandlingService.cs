using System.Text.Json;

namespace Desafio.Pages.Services;

public static class ErrorHandlingService
{
    public static string GetUserFriendlyMessage(string errorBody, int statusCode)
    {
        if (string.IsNullOrWhiteSpace(errorBody))
            return GetDefaultMessage(statusCode);

        // Tenta extrair mensagem limpa da exceção
        var cleanMessage = ExtractCleanErrorMessage(errorBody);
        if (!string.IsNullOrWhiteSpace(cleanMessage) && cleanMessage != "Ocorreu um erro durante a operação. Tente novamente.")
        {
            return cleanMessage;
        }

        // Tenta extrair mensagem de erro JSON se for uma resposta estruturada
        var jsonMessage = ExtractJsonErrorMessage(errorBody);
        if (!string.IsNullOrWhiteSpace(jsonMessage))
        {
            return jsonMessage;
        }

        // Retorna mensagem padrão baseada no status code
        return GetDefaultMessage(statusCode);
    }

    private static string ExtractCleanErrorMessage(string errorBody)
    {
        // Se contém "System.Exception:", extrai apenas a mensagem principal
        if (errorBody.Contains("System.Exception:"))
        {
            var exceptionIndex = errorBody.IndexOf("System.Exception:");
            if (exceptionIndex >= 0)
            {
                var afterException = errorBody.Substring(exceptionIndex + "System.Exception:".Length);
                var endOfMessage = afterException.IndexOf(" at ");
                
                if (endOfMessage > 0)
                {
                    return afterException.Substring(0, endOfMessage).Trim();
                }
            }
        }

        // Se contém "Exception:", tenta extrair a mensagem
        if (errorBody.Contains("Exception:"))
        {
            var exceptionIndex = errorBody.IndexOf("Exception:");
            if (exceptionIndex >= 0)
            {
                var afterException = errorBody.Substring(exceptionIndex + "Exception:".Length);
                var endOfMessage = afterException.IndexOf(" at ");
                
                if (endOfMessage > 0)
                {
                    return afterException.Substring(0, endOfMessage).Trim();
                }
            }
        }

        return string.Empty;
    }

    private static string ExtractJsonErrorMessage(string errorBody)
    {
        try
        {
            // Tenta deserializar como JSON para extrair mensagem de erro estruturada
            var jsonDoc = JsonDocument.Parse(errorBody);
            
            // Procura por campos comuns de mensagem de erro
            var messageFields = new[] { "message", "error", "detail", "title" };
            
            foreach (var field in messageFields)
            {
                if (jsonDoc.RootElement.TryGetProperty(field, out var messageElement))
                {
                    var message = messageElement.GetString();
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        return message;
                    }
                }
            }
        }
        catch
        {
            // Se não conseguir deserializar como JSON, ignora
        }

        return string.Empty;
    }

    private static string GetDefaultMessage(int statusCode)
    {
        return statusCode switch
        {
            400 => "Dados inválidos fornecidos. Verifique as informações e tente novamente.",
            401 => "Acesso não autorizado. Faça login novamente.",
            403 => "Você não tem permissão para realizar esta operação.",
            404 => "Recurso não encontrado.",
            409 => "Conflito de dados. A operação não pode ser realizada.",
            422 => "Dados inválidos. Verifique as informações fornecidas.",
            500 => "Erro interno do servidor. Tente novamente mais tarde.",
            502 => "Serviço temporariamente indisponível. Tente novamente mais tarde.",
            503 => "Serviço em manutenção. Tente novamente mais tarde.",
            _ => "Ocorreu um erro durante a operação. Tente novamente."
        };
    }
}
