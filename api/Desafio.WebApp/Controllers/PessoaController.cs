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

}
