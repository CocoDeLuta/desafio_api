using System;
using Desafio.Data;
using Desafio.Models;
using Microsoft.EntityFrameworkCore;
using Desafio.Services.Helpers;

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
        var pessoa = await _context.Pessoas.FirstOrDefaultAsync(p => p.IdPessoa == id);
        
        if (pessoa == null)
        {
            throw new Exception("Pessoa não encontrada");
        }

        return pessoa;
    }

    public async Task<PessoaModel> AdicionarPessoa(PessoaModel pessoa)
    {
        if (pessoa == null)
        {
            throw new Exception("Pessoa não pode ser nula");
        }

        if (!ValidarCpf.Validar(pessoa.Cpf))
        {
            throw new Exception("CPF inválido. Certifique-se de que ele tenha 11 caracteres numéricos.");
        }


        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();
        return pessoa;
    }

    public async Task AtualizarPessoa(PessoaModel pessoa)
    {
        if (pessoa == null)
        {
            throw new Exception("Pessoa não pode ser nula");
        }

        if (!ValidarCpf.Validar(pessoa.Cpf))
        {
            throw new Exception("CPF inválido. Certifique-se de que ele tenha 11 caracteres numéricos.");
        }

        PessoaModel PessoaExistente = await ObterPessoaPorId(pessoa.IdPessoa);

        PessoaExistente.Nome = pessoa.Nome;
        PessoaExistente.Cpf = pessoa.Cpf;
        PessoaExistente.DataNascimento = pessoa.DataNascimento;
        // não pode atualizar id obviamente

        _context.Pessoas.Update(PessoaExistente);
        await _context.SaveChangesAsync();

    }
}
