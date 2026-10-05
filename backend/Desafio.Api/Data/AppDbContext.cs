using Desafio.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Movimentacao> Movimentacoes => Set<Movimentacao>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Produto>().HasKey(p => p.CodigoProduto);
        b.Entity<Produto>().HasData(
            new Produto { CodigoProduto = 101, DescricaoProduto = "Caneta Azul", Estoque = 150 },
            new Produto { CodigoProduto = 102, DescricaoProduto = "Caderno Universitário", Estoque = 75 },
            new Produto { CodigoProduto = 103, DescricaoProduto = "Borracha Branca", Estoque = 200 },
            new Produto { CodigoProduto = 104, DescricaoProduto = "Lápis Preto HB", Estoque = 320 },
            new Produto { CodigoProduto = 105, DescricaoProduto = "Marcador de Texto Amarelo", Estoque = 90 });
    }
}
