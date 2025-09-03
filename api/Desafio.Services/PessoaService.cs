using System;
using Desafio.Data;
using Desafio.Models;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Services;

public class PessoaService
{
    private readonly EFCoreContext _context;

    public PessoaService(EFCoreContext context)
    {
        _context = context;
    }

    public async Task<List<PessoaModel>> ObterTodasPessoas()
    {
        return await _context.Pessoas.ToListAsync();
    }

    public async Task<PessoaModel> ObterPessoaPorId(int id)
    {
        return await _context.Pessoas.FirstOrDefaultAsync(p => p.IdPessoa == id);
    }

    public async Task<PessoaModel> AdicionarPessoa(PessoaModel pessoa)
    {
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();
        return pessoa;
    }

    public async Task SalvarAlteracoes()
    {
        await _context.SaveChangesAsync();
    }
}
