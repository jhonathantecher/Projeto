import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { EstacionamentoBuscaModel } from '../estacionamento/model/estacionamento-busca.model';

@Injectable({
  providedIn: 'root'
})
export class EstacionamentoService {
  baseURL = `${environment.apiUrl}/ticket`

  private _estacionamento = new BehaviorSubject<EstacionamentoBuscaModel[]>([]);
  public readonly estacionamento$: Observable<EstacionamentoBuscaModel[]> = this._estacionamento.asObservable();

  constructor(private http: HttpClient) { }

  buscarListaEstacionamento() {
    return this.http.get<EstacionamentoBuscaModel[]>(`${this.baseURL}/estacionamento`)
      .pipe(
        take(1))
      .subscribe(e => this._estacionamento.next(e));
  }
}
