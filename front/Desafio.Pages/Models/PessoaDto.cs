namespace Desafio.Pages.Models;

public class PessoaDto
{
    public int IdPessoa { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime? DataNascimento { get; set; }
}
