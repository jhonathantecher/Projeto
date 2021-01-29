import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EstacionamentoComponent } from './estacionamento/estacionamento.component';
import { FinanceiroComponent } from './financeiro/financeiro.component';
import { GerarTicketComponent } from './gerar-ticket/gerar-ticket.component';
import { TicketsComponent } from './tickets/tickets.component';
import { VeiculosComponent } from './veiculos/veiculos.component';

const routes: Routes = [
  { path: '', component: EstacionamentoComponent},
  { path: 'Gerar/Ticket', component: GerarTicketComponent},
  { path: 'Tickets', component: TicketsComponent},
  { path: 'Veiculos', component: VeiculosComponent},
  { path: 'Financeiro', component: FinanceiroComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
