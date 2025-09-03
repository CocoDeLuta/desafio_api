using System;
using Microsoft.AspNetCore.Mvc;
using Desafio.Models;
using Desafio.Services;

namespace Desafio.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoaController : ControllerBase
{

    private readonly PessoaService _pessoaService;

    public PessoaController(PessoaService pessoaService)
    {

        _pessoaService = pessoaService;
    }

    [HttpGet("ObterTodasPessoas")]
    public async Task<IActionResult> ObterTodasPessoas()
    {
        var pessoas = await _pessoaService.ObterTodasPessoas();
        return Ok(pessoas);
    }

    [HttpGet("ObterPessoaPorId/{id}")]
    public async Task<IActionResult> ObterPessoaPorId(int id)
    {
        var pessoa = await _pessoaService.ObterPessoaPorId(id);
        if (pessoa == null)
        {
            return NotFound();
        }
        return Ok(pessoa);
    }

    [HttpPost("AdicionarPessoa")]
    public async Task<IActionResult> AdicionarPessoa([FromBody] PessoaModel pessoa)
    {
        await _pessoaService.AdicionarPessoa(pessoa);
        return CreatedAtAction(nameof(ObterPessoaPorId), new { id = pessoa.IdPessoa }, pessoa);
    }

    [HttpPut("AtualizarPessoa")]
    public async Task<IActionResult> AtualizarPessoa([FromBody] PessoaModel pessoa)
    {
        var existente = await _pessoaService.ObterPessoaPorId(pessoa.IdPessoa);
        if (existente == null)
        {
            return NotFound();
        }

        existente.Nome = pessoa.Nome;
        existente.Cpf = pessoa.Cpf;
        existente.DataNascimento = pessoa.DataNascimento;

        await _pessoaService.SalvarAlteracoes();
        return Ok(existente);
    }

}
