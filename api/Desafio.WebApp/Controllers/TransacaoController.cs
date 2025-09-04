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
    public async Task<IActionResult> ObterTodasTransacoes()
    {
        var transacoes = await _transacaoService.ObterTodasTransacoes();
        return Ok(transacoes);
    }

    [HttpPost("InserirTransacao")]
    public async Task<IActionResult> InserirTransacao([FromBody] TransacaoModel transacao)
    {
        await _transacaoService.InserirTransacao(transacao);
        return CreatedAtAction(nameof(ObterTodasTransacoes), new { id = transacao.IdTransacao }, transacao);
    }

    [HttpPost("Saque/{idConta}/{valor}")]
    public async Task<IActionResult> Saque(int idConta, decimal valor)
    {
        await _transacaoService.Saque(idConta, valor);
        return NoContent();
    }

    [HttpPost("Deposito/{idConta}/{valor}")]
    public async Task<IActionResult> Deposito(int idConta, decimal valor)
    {
        await _transacaoService.Deposito(idConta, valor);
        return NoContent();
    }

    [HttpPost("Transferencia/{idContaOrigem}/{idContaDestino}/{valor}")]
    public async Task<IActionResult> Transferencia(int idContaOrigem, int idContaDestino, decimal valor)
    {
        await _transacaoService.EnviarTransferencia(idContaOrigem, idContaDestino, valor);
        return NoContent();
    }

}

