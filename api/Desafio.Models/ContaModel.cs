using System;
using System.Text.Json.Serialization;

namespace Desafio.Models;

public enum TipoConta
{
    Corrente,
    Poupanca,
    Salario
}
public class ContaModel
{
    public int IdConta { get; set; }
    public virtual PessoaModel Pessoa { get; set; }
    public decimal Saldo { get; set; }
    public decimal LimiteSaqueDiario { get; set; }
    public bool Ativo { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TipoConta TipoConta { get; set; }
    public DateTime DataCriacao { get; set; }

    // Construtores

    public ContaModel() { } // Construtor vazio para EF Core

    public ContaModel(int idConta, PessoaModel pessoa, decimal limiteSaqueDiario, TipoConta tipoConta, DateTime dataCriacao)
    {
        IdConta = idConta;
        Pessoa = pessoa;
        LimiteSaqueDiario = limiteSaqueDiario;
        Saldo = 0; // Saldo inicial é 0
        Ativo = true; // Conta é ativa por padrão
        TipoConta = tipoConta;
        DataCriacao = dataCriacao;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ContaModel other)
        {
            return other.IdConta == this.IdConta;
        }
        return false;
    }


}
