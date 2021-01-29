import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { TicketBuscaModel } from '../tickets/model/ticket-busca.model';
import { FinanceiroService } from './financeiro.service';

@Component({
  selector: 'app-financeiro',
  templateUrl: './financeiro.component.html',
  styleUrls: ['./financeiro.component.css']
})
export class FinanceiroComponent implements OnInit {
  tickets: Observable<TicketBuscaModel[]>;
  colunas: string[] = ['numero', 'placa', 'marca', 'modelo', 'dataEntrada', 'dataSaida', 'valor'];
  qtdItensLista = 0;
  total = 0;
  search: string = '';

  constructor(private financeiroService: FinanceiroService,
              private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.tickets = this.financeiroService.tickets$;
    this.financeiroService.obterTickets();
    this.somaTotal();
  }

  buscarTickets() {
    this.financeiroService.buscarTickets(this.search);
    this.somaTotal();
  }

  somaTotal() {
    this.tickets.subscribe(t => {
      this.total = 0;
      t.forEach(t => this.total += t.valor);
      this.qtdItensLista = t.length;
    });
  }
}
