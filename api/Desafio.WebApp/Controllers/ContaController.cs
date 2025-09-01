using System;
using Microsoft.AspNetCore.Mvc;
using Desafio.Models;
using Desafio.Services;

namespace Desafio.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaController : ControllerBase
{

    private readonly ContaService _contaService;

    public ContaController(ContaService contaService)
    {

        _contaService = contaService;
    }

    [HttpGet("ObterTodasContas")]
    public async Task<IActionResult> ObterTodasContas()
    {
        var contas = await _contaService.ObterTodasContas();
        return Ok(contas);
    }

    [HttpPost("InserirConta")]
    public async Task<IActionResult> InserirConta([FromBody] ContaModel conta)
    {
        if (conta == null)
        {
            return BadRequest();
        }

        await _contaService.InserirConta(conta);
        return CreatedAtAction(nameof(ObterTodasContas), new { id = conta.IdConta }, conta);
    }

    [HttpGet("ObterContaPorId/{id}")]
    public async Task<IActionResult> ObterContaPorId(int id)
    {
        var conta = await _contaService.ObterContaPorId(id);
        if (conta == null)
        {
            return NotFound();
        }
        return Ok(conta);
    }

    [HttpPut("AtualizarConta")]
    public async Task<IActionResult> AtualizarConta([FromBody] ContaModel conta)
    {
        if (conta == null)
        {
            return BadRequest();
        }

        await _contaService.AtualizarConta(conta);
        return NoContent();
    }

    [HttpDelete("DeletarConta/{id}")]
    public async Task<IActionResult> DeletarConta(int id)
    {
        await _contaService.DeletarConta(id);
        return NoContent();
    }



}
