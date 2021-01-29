import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { filter } from 'rxjs/operators';
import { DialogConfirmacaoComponent } from '../dialogs/confirmacao/dialog-confirmacao.component';
import { TicketBuscaModel } from './model/ticket-busca.model';
import { TicketService } from './tickets.service';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent implements OnInit {

  tickets: Observable<TicketBuscaModel[]>;
  colunas: string[] = ['numero', 'placa', 'marca', 'modelo', 'dataEntrada', 'dataSaida', 'valor', 'finalizar', 'deletar'];
  qtdItensLista = 0;
  search: string = '';

  constructor(private ticketService: TicketService,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    this.ticketService.obterTickets();
    this.tickets = this.ticketService.tickets$;

    this.tickets.subscribe(t => this.qtdItensLista = t.length);
  }

  buscarTickets() {
    this.ticketService.buscarTickets(this.search);
  }

  finalizarTicket(numero: number) {
    let dialogRef = this.dialog.open(DialogConfirmacaoComponent, {
      data: { title: "Ticket's", msg: 'Tem certeza que deseja Finalizar o Ticket?' }
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter( res => res == true))
      .subscribe(r => {
        this.ticketService.finalizarTicket(numero);
      });
  }

  deletarTicket(numero: number) {
    let dialogRef = this.dialog.open(DialogConfirmacaoComponent, {
      data: { title: "Ticket's", msg: 'Tem certeza que deseja Excluir o Ticket?' }
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter(
          res => res == true))
      .subscribe(r => {
        this.ticketService.deletarTicket(numero);
      });
  }
}
