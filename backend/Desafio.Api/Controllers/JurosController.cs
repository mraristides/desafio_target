using Microsoft.AspNetCore.Mvc;

namespace Desafio.Api.Controllers;

public record JurosRequest(decimal Valor, DateTime DataVencimento);
public record JurosResponse(decimal Valor, DateTime DataVencimento, DateTime DataCalculo,
    int DiasAtraso, decimal Juros, decimal ValorAtualizado);

[ApiController]
[Route("api/[controller]")]
public class JurosController : ControllerBase
{
    private const decimal TaxaDiaria = 0.025m; // 2,5% ao dia (juros simples)

    [HttpPost("calcular")]
    public ActionResult<JurosResponse> Calcular([FromBody] JurosRequest req)
    {
        if (req.Valor <= 0)
            return BadRequest(new { erro = "Valor deve ser maior que zero." });

        var hoje = DateTime.Today;
        var dias = Math.Max(0, (hoje - req.DataVencimento.Date).Days);
        var juros = Math.Round(req.Valor * TaxaDiaria * dias, 2);

        return Ok(new JurosResponse(req.Valor, req.DataVencimento.Date, hoje, dias, juros, req.Valor + juros));
    }
}
