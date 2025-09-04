using System;
using Desafio.Data;
using Desafio.Models;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Services;


public class ContaService
{
    private readonly EFCoreContext _context;

    public ContaService(EFCoreContext context)
    {
        _context = context;
    }

    // metodos que o controller vai chamar
    // listar todas as contas
    public async Task<List<ContaModel>> ObterTodasContas()
    {
        return await _context.Contas.ToListAsync();
    }

    // pegar conta por id
    public async Task<ContaModel> ObterContaPorId(int id)
    {
        // Verifica se a conta existe antes de retornar
        ContaModel? conta = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == id);
        if (conta == null)
        {
            throw new Exception("Conta não encontrada");
        }

        return conta;
    }
    // criar conta
    public async Task InserirConta(ContaModel conta)
    {
        // Verifica se a pessoa existe antes de atribuir
        PessoaModel? pessoa = await _context.Pessoas.FirstOrDefaultAsync(p => p.IdPessoa == conta.Pessoa.IdPessoa);
        if (pessoa == null)
        {
            throw new Exception("Pessoa não encontrada");
        }
        
        conta.Pessoa = pessoa;
        _context.Contas.Add(conta);
        await _context.SaveChangesAsync();
    }



    // atualizar conta
    public async Task AtualizarConta(ContaModel conta)
    {
        // Verifica se a conta existe antes de atualizar
        ContaModel? existingConta = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == conta.IdConta);
        if (existingConta == null)
        {
            throw new Exception("Conta não encontrada");
        }

        existingConta.LimiteSaqueDiario = conta.LimiteSaqueDiario;
        existingConta.Ativo = conta.Ativo;
        existingConta.TipoConta = conta.TipoConta;
        // não pode atualizar saldo, pessoa, data criação

        _context.Contas.Update(existingConta);
        await _context.SaveChangesAsync();
    }

    // deletar conta
    public async Task DeletarConta(int id)
    {
        // Ajusta o tipo para anulável
        ContaModel? existingConta = await _context.Contas.FirstOrDefaultAsync(c => c.IdConta == id);
        if (existingConta == null)
        {
            throw new Exception("Conta não encontrada");
        }
        
        _context.Contas.Remove(existingConta);
        await _context.SaveChangesAsync();
    }

}
