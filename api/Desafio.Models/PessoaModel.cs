using System;

namespace Desafio.Models;

public class PessoaModel
{
    public int IdPessoa { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public DateTime DataNascimento { get; set; }

    // Contrutores

    public PessoaModel() { } // Construtor vazio para EF Core

    public PessoaModel(int idPessoa, string nome, string cpf, DateTime dataNascimento)
    {
        IdPessoa = idPessoa;
        Nome = nome;
        Cpf = cpf;
        DataNascimento = dataNascimento;
    }

    public override bool Equals(object? obj)
    {
        if (obj is PessoaModel other)
        {
            return other.IdPessoa == this.IdPessoa;
        }
        return false;
    }
}
