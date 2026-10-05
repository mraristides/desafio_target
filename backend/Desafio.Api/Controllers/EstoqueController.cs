using Desafio.Api.Data;
using Desafio.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstoqueController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Listar() =>
        Ok(await db.Produtos.OrderBy(p => p.CodigoProduto).ToListAsync());

    [HttpGet("movimentacoes")]
    public async Task<ActionResult<IEnumerable<Movimentacao>>> Historico() =>
        Ok(await db.Movimentacoes.OrderByDescending(m => m.Id).Take(50).ToListAsync());

    [HttpPost("movimentacoes")]
    public async Task<ActionResult<MovimentacaoResponse>> Movimentar([FromBody] MovimentacaoRequest req)
    {
        var tipo = req.Tipo?.Trim().ToLowerInvariant();
        if (tipo is not ("entrada" or "saida" or "saída"))
            return BadRequest(new { erro = "Tipo deve ser 'Entrada' ou 'Saida'." });
        if (req.Quantidade <= 0)
            return BadRequest(new { erro = "Quantidade deve ser maior que zero." });
        if (string.IsNullOrWhiteSpace(req.Descricao))
            return BadRequest(new { erro = "Informe a descrição da movimentação." });

        var produto = await db.Produtos.FindAsync(req.CodigoProduto);
        if (produto is null)
            return NotFound(new { erro = $"Produto {req.CodigoProduto} não encontrado." });

        var entrada = tipo == "entrada";
        if (!entrada && produto.Estoque < req.Quantidade)
            return BadRequest(new { erro = $"Estoque insuficiente. Disponível: {produto.Estoque}." });

        produto.Estoque += entrada ? req.Quantidade : -req.Quantidade;

        var mov = new Movimentacao
        {
            CodigoProduto = produto.CodigoProduto,
            Tipo = entrada ? "Entrada" : "Saida",
            Quantidade = req.Quantidade,
            Descricao = req.Descricao,
            DataHora = DateTime.UtcNow,
            SaldoApos = produto.Estoque
        };
        db.Movimentacoes.Add(mov);
        await db.SaveChangesAsync();

        return Ok(new MovimentacaoResponse(mov.Id, produto.CodigoProduto, produto.DescricaoProduto,
            mov.Tipo, mov.Descricao, mov.Quantidade, produto.Estoque));
    }
}
