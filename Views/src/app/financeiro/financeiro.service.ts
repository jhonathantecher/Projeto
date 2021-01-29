import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { TicketBuscaModel } from '../tickets/model/ticket-busca.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class FinanceiroService {
  baseURL = `${environment.apiUrl}/ticket`

  private _tickets = new BehaviorSubject<TicketBuscaModel[]>([]);
  public readonly tickets$: Observable<TicketBuscaModel[]> = this._tickets.asObservable();

  constructor(private http: HttpClient,
              private snackBar: MatSnackBar) { }

  obterTickets() {
    return this.http.get<TicketBuscaModel[]>(`${this.baseURL}/tickets/finalizados`)
      .pipe(
        take(1))
      .subscribe(v => this._tickets.next(v));
  }

  buscarTickets(pesquisa: string) {
    return this.http.get<TicketBuscaModel[]>(`${this.baseURL}/tickets/finalizados/${pesquisa}`)
      .pipe(
        take(1))
      .subscribe(t => this._tickets.next(t));
  }
}
