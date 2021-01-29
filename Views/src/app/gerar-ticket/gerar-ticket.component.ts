import { Component, OnDestroy, OnInit } from '@angular/core';
import { GerarTicketFormService } from './gerar-ticket-form.service';
import { GerarTicketService } from './gerar-ticket.service';
import { TicketCadastroModel } from './model/ticket-cadastro.model';

@Component({
  selector: 'app-gerar-ticket',
  templateUrl: './gerar-ticket.component.html',
  styleUrls: ['./gerar-ticket.component.css']
})
export class GerarTicketComponent implements OnInit, OnDestroy {

  constructor(private gerarTicketService: GerarTicketService,
              public gerarTicketFormService: GerarTicketFormService) { }

  ngOnInit(): void { }

  ngOnDestroy(): void {
    this.gerarTicketFormService.form.get('veiculo').reset();
    this.gerarTicketFormService.form.get('veiculo').enable();
    this.gerarTicketFormService.disableForm = true;
   }

  salvarTicket() {
    var ticket: TicketCadastroModel = this.gerarTicketFormService.form.get('veiculo').value;
    this.gerarTicketService.salvarTicket(ticket);
  }

  buscarVeiculo() {
    this.gerarTicketService.buscarVeiculo();
  }

  bloquearBusca() {
    this.gerarTicketFormService.form.get('veiculo').get('marca').reset();
    this.gerarTicketFormService.form.get('veiculo').get('modelo').reset();
    this.gerarTicketFormService.form.get('veiculo').get('nome').reset();
    this.gerarTicketFormService.form.get('veiculo').enable();

    this.gerarTicketFormService.disableForm = true;
  }
}
