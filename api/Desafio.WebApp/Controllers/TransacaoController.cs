using System;
using Microsoft.AspNetCore.Mvc;
using Desafio.Models;
using Desafio.Services;

namespace Desafio.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacaoController : ControllerBase
{

    private readonly TransacaoService _transacaoService;

    public TransacaoController(TransacaoService transacaoService)
    {

        _transacaoService = transacaoService;
    }

    [HttpGet("ObterTodasTransacoes")]
    public async Task<IActionResult> GetAll()
    {
        var transacoes = await _transacaoService.GetAll();
        return Ok(transacoes);
    }

    [HttpPost("InserirTransacao")]
    public async Task<IActionResult> Insert([FromBody] TransacaoModel transacao)
    {
        if (transacao == null)
        {
            return BadRequest();
        }

        await _transacaoService.Insert(transacao);
        return CreatedAtAction(nameof(GetAll), new { id = transacao.IdTransacao }, transacao);
    }

    [HttpPost("Saque/{idConta}/{valor}")]
    public async Task<IActionResult> Saque(int idConta, decimal valor)
    {
        try
        {
            await _transacaoService.Saque(idConta, valor);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Deposito/{idConta}/{valor}")]
    public async Task<IActionResult> Deposito(int idConta, decimal valor)
    {
        try
        {
            await _transacaoService.Deposito(idConta, valor);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Transferencia/{idContaOrigem}/{idContaDestino}/{valor}")]
    public async Task<IActionResult> Transferencia(int idContaOrigem, int idContaDestino, decimal valor)
    {
        try
        {
            await _transacaoService.EnviarTransferencia(idContaOrigem, idContaDestino, valor);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}

