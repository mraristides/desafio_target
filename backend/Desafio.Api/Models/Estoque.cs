using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio.Api.Models;

public class Produto
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CodigoProduto { get; set; }
    public string DescricaoProduto { get; set; } = "";
    public int Estoque { get; set; }
}

public class Movimentacao
{
    public int Id { get; set; }                       // identificador único (identity)
    public int CodigoProduto { get; set; }
    public string Tipo { get; set; } = "";            // Entrada | Saida
    public int Quantidade { get; set; }
    public string Descricao { get; set; } = "";       // descrição do tipo da movimentação
    public DateTime DataHora { get; set; }
    public int SaldoApos { get; set; }
}

public record MovimentacaoRequest(int CodigoProduto, string Tipo, int Quantidade, string Descricao);

public record MovimentacaoResponse(
    int IdMovimentacao, int CodigoProduto, string DescricaoProduto,
    string Tipo, string Descricao, int Quantidade, int EstoqueFinal);
