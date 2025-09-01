using System;
using System.Text.Json.Serialization;

namespace Desafio.Models;

public enum TipoTransacao
{
    Saque,
    Deposito,
    TransferenciaEnviada,
    TransferenciaRecebida
}
public class TransacaoModel
{
    public int IdTransacao { get; set; }
    public virtual ContaModel Conta { get; set; }
    public decimal Valor { get; set; }
    public virtual DateTime DataTransacao { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TipoTransacao TipoTransacao { get; set; }

    // Construtores
    public TransacaoModel() { } // Construtor vazio para EF

    public TransacaoModel(int idTransacao, ContaModel conta, decimal valor, DateTime dataTransacao, TipoTransacao tipoTransacao)
    {
        IdTransacao = idTransacao;
        Conta = conta;
        Valor = valor;
        DataTransacao = dataTransacao;
        TipoTransacao = tipoTransacao;
    }

    public override bool Equals(object? obj)
    {
        if (obj is TransacaoModel other)
        {
            return other.IdTransacao == this.IdTransacao;
        }
        return false;
    }
}
