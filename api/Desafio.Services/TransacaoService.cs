using System;
using System.Linq;
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

    public async Task<List<TransacaoModel>> ObterTodasTransacoes()
    {
        return await _context.Transacoes.ToListAsync();
    }

    // insert so pra testar no backend
    public async Task InserirTransacao(TransacaoModel transacao)
    {
        // checagem de null  pra ficar mais facil de debugar
        if (transacao == null)
        {
            throw new Exception("Transação não pode ser nula");
        }

        // try catch so pra garantir que qualquer erro seja capturado
        // fica mais facil de debugar
        try
        {
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao inserir transação: " + ex.Message);
        }
    }

    // saque
    public async Task Saque(int idConta, decimal valor)
    {
        ContaModel? conta = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == idConta);

        // validações usando métodos auxiliares
        ValidarConta(conta, "Conta não encontrada");
        ValidarTransferencia(valor, conta, conta);

        await ValidarLimiteDiario(idConta, valor, conta.LimiteSaqueDiario);

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
        ContaModel? conta = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == idConta);
        
        // validação via método auxiliar (verifica também se está ativo)
        ValidarConta(conta, "Conta não encontrada");

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
        ContaModel? conta = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == idConta);
        if (conta == null)
        {
            throw new Exception("Conta de destino não encontrada");
        }

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
        ContaModel? contaOrigem = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == idContaOrigem);
        ContaModel? contaDestino = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == idContaDestino);

        // Validações
        ValidarConta(contaOrigem, "Conta de origem não encontrada");
        ValidarConta(contaDestino, "Conta de destino não encontrada");
        ValidarTransferencia(valor, contaOrigem, contaDestino);

        await ValidarLimiteDiario(idContaOrigem, valor, contaOrigem.LimiteSaqueDiario);

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

    private void ValidarConta(ContaModel? conta, string mensagemErro)
    {
        if (conta == null)
        {
            throw new Exception(mensagemErro);
        }

        if (!conta.Ativo)
        {
            throw new Exception("Conta inativa");
        }

    }

    private void ValidarTransferencia(decimal valor, ContaModel contaOrigem, ContaModel contaDestino)
    {
        if (valor <= 0)
        {
            throw new Exception("Valor de transferência deve ser positivo");
        }

        if (valor > contaOrigem.Saldo)
        {
            throw new Exception("Saldo insuficiente na conta de origem");
        }

        // se a conta for do tipo salario, nao pode transferir para contas de outras pessoas
        if(contaOrigem.TipoConta == TipoConta.Salario && contaOrigem.Pessoa.IdPessoa != contaDestino.Pessoa.IdPessoa)
        {
            throw new Exception("Conta salário só pode transferir para contas do mesmo titular");
        }

    }

    private async Task ValidarLimiteDiario(int idConta, decimal valor, decimal limiteSaqueDiario)
    {
        // somar as transações de saque e transferência enviadas do dia
        // para verificar se a transferencia nova passa o limite diario ou nao
        DateTime hoje = DateTime.Now.Date;
        var totalHoje = await _context.Transacoes
            .Where(t => t.Conta.IdConta == idConta && t.DataTransacao.Date == hoje &&
                (t.TipoTransacao == TipoTransacao.Saque || t.TipoTransacao == TipoTransacao.TransferenciaEnviada))
            .SumAsync(t => (decimal?)t.Valor) ?? 0m; // se nao houver transacoes, retorna 0

        if (totalHoje + valor > limiteSaqueDiario)
        {
            throw new Exception("Valor excede o limite diário disponível");
        }
    }
}
