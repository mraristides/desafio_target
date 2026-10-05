import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  styleUrl: './app.component.css',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  aba = 1;
  erro = '';

  vendasJson = '';
  comissoes: any[] = [];

  produtos: any[] = [];
  historico: any[] = [];
  mov: any = { codigoProduto: 101, tipo: 'Entrada', quantidade: 1, descricao: '' };
  resultadoMov: any = null;

  juros: any = { valor: 1000, dataVencimento: '' };
  resultadoJuros: any = null;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.http.get('assets/vendas.json', { responseType: 'text' })
      .subscribe(t => this.vendasJson = t);
  }

  private falha(e: any) {
    this.erro = e?.error?.erro ?? 'Erro ao chamar a API.';
  }

  calcularComissoes() {
    this.erro = '';
    let corpo: any;
    try { corpo = JSON.parse(this.vendasJson); } catch { this.erro = 'JSON inválido.'; return; }
    this.http.post<any[]>('/api/comissoes/calcular', corpo)
      .subscribe({ next: r => this.comissoes = r, error: e => this.falha(e) });
  }

  carregarEstoque() {
    this.http.get<any[]>('/api/estoque').subscribe(r => this.produtos = r);
    this.http.get<any[]>('/api/estoque/movimentacoes').subscribe(r => this.historico = r);
  }

  movimentar() {
    this.erro = ''; this.resultadoMov = null;
    this.http.post('/api/estoque/movimentacoes', this.mov).subscribe({
      next: r => { this.resultadoMov = r; this.carregarEstoque(); },
      error: e => this.falha(e)
    });
  }

  calcularJuros() {
    this.erro = ''; this.resultadoJuros = null;
    this.http.post('/api/juros/calcular', this.juros)
      .subscribe({ next: r => this.resultadoJuros = r, error: e => this.falha(e) });
  }
}
