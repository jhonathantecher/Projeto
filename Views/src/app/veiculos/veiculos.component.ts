import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';
import { DialogAtualizarVeiculoComponent } from '../dialogs/atualizar-veiculo/dialog-atualizar-veiculo.component';
import { DialogConfirmacaoComponent } from '../dialogs/confirmacao/dialog-confirmacao.component';
import { VeiculoModel } from './model/veiculo.model';
import { VeiculoService } from './veiculo.service';

@Component({
  selector: 'app-veiculos',
  templateUrl: './veiculos.component.html',
  styleUrls: ['./veiculos.component.css']
})
export class VeiculosComponent implements OnInit {
  veiculos: Observable<VeiculoModel[]>;
  colunas: string[] = ['placa', 'marca', 'modelo', 'nome', 'atualizar', 'deletar'];
  qtdItensLista = 0;
  search: string = '';

  constructor(private veiculoService: VeiculoService,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    this.veiculoService.obterVeiculos();
    this.veiculos = this.veiculoService.veiculos$;

    this.veiculos.subscribe(v => this.qtdItensLista = v.length);
  }

  buscarVeiculos() {
    this.veiculoService.buscarVeiculos(this.search);
  }

  atualizarVeiculo(veiculo: VeiculoModel) {
    this.dialog.open(DialogAtualizarVeiculoComponent, {
      data: veiculo
    });
  }

  deletarVeiculo(placa: string) {
    let dialogRef = this.dialog.open(DialogConfirmacaoComponent, {
      data: { title: "Veículos", msg: 'Tem certeza que deseja Excluir o Veículo?' }
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter(
          res => res == true),
        // switchMap(() => {
        //   return this.veiculoService.deletarVeiculo(placa);
        // })
        )
      .subscribe(() => {
        this.veiculoService.deletarVeiculo(placa);
      });
  }
}
