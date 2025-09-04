namespace Desafio.Pages.Services;

public class MessageService
{
    public string Message { get; private set; } = string.Empty;
    public string MessageClass { get; private set; } = "alert-info";
    public bool HasMessage => !string.IsNullOrWhiteSpace(Message);

    public event Action? OnMessageChanged;

    public void ShowSuccess(string message)
    {
        Message = message;
        MessageClass = "alert-success";
        OnMessageChanged?.Invoke();
    }

    public void ShowError(string message)
    {
        Message = message;
        MessageClass = "alert-danger";
        OnMessageChanged?.Invoke();
    }

    public void ShowInfo(string message)
    {
        Message = message;
        MessageClass = "alert-info";
        OnMessageChanged?.Invoke();
    }

    public void Clear()
    {
        Message = string.Empty;
        MessageClass = "alert-info";
        OnMessageChanged?.Invoke();
    }
}
