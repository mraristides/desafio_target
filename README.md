# Desafio – Angular + .NET 8 API + SQL Server (Docker)

    docker compose up --build

- App (Angular):  http://localhost:4200
- API / Swagger:  http://localhost:5000/swagger

## Endpoints
| Desafio | Método | Rota |
|---|---|---|
| 1 Comissões | POST | /api/comissoes/calcular |
| 2 Estoque   | GET  | /api/estoque |
| 2 Estoque   | POST | /api/estoque/movimentacoes |
| 2 Estoque   | GET  | /api/estoque/movimentacoes (histórico) |
| 3 Juros     | POST | /api/juros/calcular |

## Premissas
- Comissão: <100 = 0%; 100 a <500 = 1%; >=500 = 5% (por venda, somada por vendedor).
- Juros: 2,5% ao dia sobre o valor, juros simples, dias corridos de atraso até hoje (0 se não vencido).
- Estoque: Id da movimentação é identity do SQL Server; saída maior que o saldo é rejeitada.
