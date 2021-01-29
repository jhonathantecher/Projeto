import { Injectable } from '@angular/core';

import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

import { catchError, take } from 'rxjs/operators';
import { VeiculoModel } from './model/veiculo.model';
import { environment } from 'src/environments/environment';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class VeiculoService {
  baseURL = `${environment.apiUrl}/veiculo`

  private _veiculos = new BehaviorSubject<VeiculoModel[]>([]);
  public readonly veiculos$: Observable<VeiculoModel[]> = this._veiculos.asObservable();

  constructor(private http: HttpClient,
    private snackBar: MatSnackBar) { }

  obterVeiculos() {
    return this.http.get<VeiculoModel[]>(this.baseURL)
      .pipe(
        take(1))
      .subscribe(v => this._veiculos.next(v));
  }

  buscarVeiculos(pesquisa: string) {
    return this.http.get<VeiculoModel[]>(`${this.baseURL}/${pesquisa}`)
      .pipe(
        take(1))
      .subscribe(v => this._veiculos.next(v));
  }

  atualizarVeiculo(veiculo: VeiculoModel) {
    return this.http.put<VeiculoModel>(this.baseURL, veiculo)
      .pipe(
        take(1),
        catchError((error: HttpErrorResponse) => {
          this.abrirSnackBarError(error.error.split(':', 2)[1].split('at', 1));
          throw error;
        }))
      .subscribe(() => {
        let veiculos = this._veiculos.getValue()
          .map(function (v) {
            return v.placa == veiculo.placa ? veiculo : v;
          });

        this._veiculos.next(veiculos);
        this.abrirSnackBarSucesso();
      });
  }

  deletarVeiculo(placa: string) {
    return this.http.delete<VeiculoModel>(`${this.baseURL}/${placa}`)
      .pipe(
        take(1),
        catchError((error: HttpErrorResponse) => {
          this.abrirSnackBarError(error.error.split(':', 2)[1].split('at', 1));
          throw error;
        }))
      .subscribe(() => {
        let veiculos = this._veiculos.getValue();
        veiculos = veiculos.filter(v => v.placa !== placa);
        this._veiculos.next(veiculos);

        this.snackBar.open('Excluído com Sucesso!', '', {
          duration: 3000,
          panelClass: ['green-snackbar', 'veiculo-snackbar'],
        });
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
