namespace Desafio.Pages.Models;

public class TransacaoDto
{
    public int IdTransacao { get; set; }
    public ContaDto Conta { get; set; } = new();
    public decimal Valor { get; set; }
    public string TipoTransacao { get; set; } = string.Empty;
    public DateTime DataTransacao { get; set; }
}
