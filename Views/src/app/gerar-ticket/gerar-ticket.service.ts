import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { TicketCadastroModel } from './model/ticket-cadastro.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GerarTicketFormService } from './gerar-ticket-form.service';
import { VeiculoModel } from '../veiculos/model/veiculo.model';

@Injectable({
  providedIn: 'root'
})
export class GerarTicketService {
  baseURL = `${environment.apiUrl}/ticket`

  constructor(private http: HttpClient,
    private gerarTicketFormService: GerarTicketFormService,
    private snackBar: MatSnackBar) { }

  salvarTicket(ticket: TicketCadastroModel) {
    return this.http.post<TicketCadastroModel>(`${this.baseURL}`, ticket)
      .pipe(
        take(1),
        catchError((error: HttpErrorResponse) => {
          this.abrirSnackBarError(error.error.split(':', 2)[1].split('at', 1));
          throw error;
        }))
      .subscribe(() => {
        this.gerarTicketFormService.form.get('veiculo').reset();
        this.gerarTicketFormService.form.get('veiculo').enable();
        this.gerarTicketFormService.disableForm = true;
        this.abrirSnackBarSucesso();
      });
  }

  buscarVeiculo() {
    return this.http.get<VeiculoModel>(`${this.baseURL}/${this.gerarTicketFormService.form.get('veiculo').get('placa').value}`)
      .pipe(
        take(1))
      .subscribe(v => {
        if(v != null){
          this.gerarTicketFormService.form.get('veiculo').patchValue(v);
          this.gerarTicketFormService.form.get('veiculo').get('marca').disable();
          this.gerarTicketFormService.form.get('veiculo').get('modelo').disable();
          this.gerarTicketFormService.form.get('veiculo').get('nome').disable();
        }
        this.gerarTicketFormService.disableForm = false;
      });
  }

  abrirSnackBarSucesso() {
    this.snackBar.open('Salvo com Sucesso!', '', {
      duration: 3000,
      panelClass: ['green-snackbar', 'GerarTicket-snackbar'],
    });
  }

  abrirSnackBarError(msg: string) {
    this.snackBar.open(msg, '', {
      duration: 3000,
      panelClass: ['red-snackbar', 'GerarTicket-snackbar'],
    });
  }
}
