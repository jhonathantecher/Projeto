import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { TicketBuscaModel } from './model/ticket-busca.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  baseURL = `${environment.apiUrl}/ticket`

  private _tickets = new BehaviorSubject<TicketBuscaModel[]>([]);
  public readonly tickets$: Observable<TicketBuscaModel[]> = this._tickets.asObservable();

  constructor(private http: HttpClient,
    private snackBar: MatSnackBar) { }

  obterTickets() {
    return this.http.get<TicketBuscaModel[]>(`${this.baseURL}/tickets`)
      .pipe(
        take(1))
      .subscribe(v => this._tickets.next(v));
  }

  buscarTickets(pesquisa: string) {
    return this.http.get<TicketBuscaModel[]>(`${this.baseURL}/tickets/${pesquisa}`)
      .pipe(
        take(1))
      .subscribe(t => this._tickets.next(t));
  }

  finalizarTicket(numero: number) {
    return this.http.put<TicketBuscaModel>(`${this.baseURL}/${numero}`, null)
      .pipe(
        take(1))
      .subscribe(() => {
        this.obterTickets();
        this.snackBar.open('Finalizado com Sucesso!', '', {
          duration: 3000,
          panelClass: ['green-snackbar', 'veiculo-snackbar'],
        });
      })
  }

  deletarTicket(numero: number) {
    return this.http.delete<TicketBuscaModel>(`${this.baseURL}/${numero}`)
      .pipe(
        take(1))
      .subscribe(() => {
        var tickets = this._tickets.getValue();
        tickets = tickets.filter(v => v.numero !== numero);
        this._tickets.next(tickets);

        this.snackBar.open('Excluído com Sucesso!', '', {
          duration: 3000,
          panelClass: ['green-snackbar', 'veiculo-snackbar'],
        });
      })
  }
}
