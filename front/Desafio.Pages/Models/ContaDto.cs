namespace Desafio.Pages.Models;

public class ContaDto
{
    public int IdConta { get; set; }
    public PessoaDto? Pessoa { get; set; }
    public decimal Saldo { get; set; }
    public decimal LimiteSaqueDiario { get; set; }
    public bool Ativo { get; set; }
    public string TipoConta { get; set; } = "Corrente";
    public DateTime DataCriacao { get; set; }
}
