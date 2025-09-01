using System;
using Desafio.Data;
using Desafio.Models;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Services;

public class TransacaoService
{
    private readonly EFCoreContext _context;

    public TransacaoService(EFCoreContext context)
    {
        _context = context;
    }

    public async Task<List<TransacaoModel>> GetAll()
    {
        return await _context.Transacoes.ToListAsync();
    }

    public async Task Insert(TransacaoModel transacao)
    {
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
    }

    // saque
    public async Task Saque(int idConta, decimal valor)
    {
        ContaModel conta = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == idConta);

        // validações
        // - MOVER PARA METODOS TALVEZ? so que ai eriam muitas chamadas ao banco, pioraria a performance?
        if (conta == null)
        {
            throw new Exception("Conta não encontrada");
        }

        if (!conta.Ativo)
        {
            throw new Exception("Conta inativa");
        }

        if (valor <= 0)
        {
            throw new Exception("Valor de saque deve ser positivo");
        }

        if (valor > conta.LimiteSaqueDiario)
        {
            throw new Exception("Valor de saque excede o limite diário");
        }

        if (valor > conta.Saldo)
        {
            throw new Exception("Saldo insuficiente");
        }

        // se tudo ok, realiza o saque
        conta.Saldo -= valor;

        // registra a transação
        TransacaoModel transacao = new TransacaoModel
        {
            Conta = conta,
            Valor = valor,
            DataTransacao = DateTime.Now,
            TipoTransacao = TipoTransacao.Saque
        };

        // salva as mudanças
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
    }

    // deposito
    public async Task Deposito(int idConta, decimal valor)
    {
        ContaModel conta = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == idConta);

        // validações
        if (conta == null)
        {
            throw new Exception("Conta não encontrada");
        }

        if (!conta.Ativo)
        {
            throw new Exception("Conta inativa");
        }

        if (valor <= 0)
        {
            throw new Exception("Valor de depósito deve ser positivo");
        }

        // se tudo ok, realiza o depósito
        conta.Saldo += valor;

        // registra a transação
        TransacaoModel transacao = new TransacaoModel
        {
            Conta = conta,
            Valor = valor,
            DataTransacao = DateTime.Now,
            TipoTransacao = TipoTransacao.Deposito
        };

        // salva as mudanças
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
    }

    // receber transferencia
    public async Task ReceberTransferencia(int idConta, decimal valor)
    {
        ContaModel conta = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == idConta);

        // validações estao na parte do envio da transferencia
        // se tudo ok, realiza a transferência
        conta.Saldo += valor;

        // registra a transação
        TransacaoModel transacao = new TransacaoModel
        {
            Conta = conta,
            Valor = valor,
            DataTransacao = DateTime.Now,
            TipoTransacao = TipoTransacao.TransferenciaRecebida
        };

        // salva as mudanças
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
    }

    // enviar transferencia
    public async Task EnviarTransferencia(int idContaOrigem, int idContaDestino, decimal valor)
    {
        ContaModel contaOrigem = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == idContaOrigem);
        ContaModel contaDestino = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == idContaDestino);

        // validações
        if (contaOrigem == null)
        {
            throw new Exception("Conta de origem não encontrada");
        }

        if (contaDestino == null)
        {
            throw new Exception("Conta de destino não encontrada");
        }

        if (!contaOrigem.Ativo)
        {
            throw new Exception("Conta de origem inativa");
        }

        if (!contaDestino.Ativo)
        {
            throw new Exception("Conta de destino inativa");
        }

        if (valor <= 0)
        {
            throw new Exception("Valor de transferência deve ser positivo");
        }

        if (valor > contaOrigem.Saldo)
        {
            throw new Exception("Saldo insuficiente na conta de origem");
        }

        // se tudo ok, realiza a transferência
        contaOrigem.Saldo -= valor;
        
        // conta destino recebe a transferencia
        Task receberTask = ReceberTransferencia(idContaDestino, valor);
        await receberTask;

        // registra a transação na conta de origem
        TransacaoModel transacaoOrigem = new TransacaoModel
        {
            Conta = contaOrigem,
            Valor = valor,
            DataTransacao = DateTime.Now,
            TipoTransacao = TipoTransacao.TransferenciaEnviada
        };

        // salva as mudanças
        _context.Transacoes.Add(transacaoOrigem);
        _context.Contas.Update(contaOrigem);
        await _context.SaveChangesAsync();
    }
}
