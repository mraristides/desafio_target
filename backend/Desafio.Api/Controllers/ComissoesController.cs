using Microsoft.AspNetCore.Mvc;

namespace Desafio.Api.Controllers;

public record Venda(string Vendedor, decimal Valor);
public record VendasRequest(List<Venda> Vendas);
public record ComissaoVendedor(string Vendedor, int QtdVendas, decimal TotalVendido, decimal Comissao);

[ApiController]
[Route("api/[controller]")]
public class ComissoesController : ControllerBase
{
    // Regra: < 100 => 0% | < 500 => 1% | >= 500 => 5%
    public static decimal CalcularComissao(decimal valor) =>
        valor < 100m ? 0m :
        valor < 500m ? valor * 0.01m :
        valor * 0.05m;

    [HttpPost("calcular")]
    public ActionResult<IEnumerable<ComissaoVendedor>> Calcular([FromBody] VendasRequest req)
    {
        if (req?.Vendas is null || req.Vendas.Count == 0)
            return BadRequest(new { erro = "Informe ao menos uma venda." });

        var resultado = req.Vendas
            .GroupBy(v => v.Vendedor)
            .Select(g => new ComissaoVendedor(
                g.Key,
                g.Count(),
                g.Sum(v => v.Valor),
                Math.Round(g.Sum(v => CalcularComissao(v.Valor)), 2)))
            .OrderBy(r => r.Vendedor);

        return Ok(resultado);
    }
}
