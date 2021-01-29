import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatMenuModule } from '@angular/material/menu';
import { MatListModule } from '@angular/material/list';
import { AppRoutingModule } from './app-routing.module';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule } from '@angular/common/http';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { EstacionamentoComponent } from './estacionamento/estacionamento.component';
import { GerarTicketComponent } from './gerar-ticket/gerar-ticket.component';
import { TicketsComponent } from './tickets/tickets.component';
import { VeiculosComponent } from './veiculos/veiculos.component';
import { DialogConfirmacaoComponent } from './dialogs/confirmacao/dialog-confirmacao.component';
import { DialogAtualizarVeiculoComponent } from './dialogs/atualizar-veiculo/dialog-atualizar-veiculo.component';
import { FinanceiroComponent } from './financeiro/financeiro.component';


@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    EstacionamentoComponent,
    GerarTicketComponent,
    TicketsComponent,
    VeiculosComponent,
    DialogConfirmacaoComponent,
    DialogAtualizarVeiculoComponent,
    FinanceiroComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatSidenavModule,
    MatMenuModule,
    MatListModule,
    AppRoutingModule,
    MatCardModule,
    MatTableModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    FlexLayoutModule,
    MatExpansionModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTooltipModule,
    HttpClientModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
