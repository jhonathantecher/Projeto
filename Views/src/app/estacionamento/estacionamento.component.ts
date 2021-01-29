import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { EstacionamentoService } from './estacionamento.service';
import { EstacionamentoBuscaModel } from './model/estacionamento-busca.model';

@Component({
  selector: 'app-estacionamento',
  templateUrl: './estacionamento.component.html',
  styleUrls: ['./estacionamento.component.css']
})
export class EstacionamentoComponent implements OnInit {

  estacionamento: Observable<EstacionamentoBuscaModel[]>;
  colunas: string[] = ['placa','marca','modelo','nome', 'dataEntrada'];

  capacidade = 50;
  disponiveis = 50;
  ocupadas = 0;

  constructor(private estacionamentoService: EstacionamentoService) { }

  ngOnInit(): void {
    this.estacionamentoService.buscarListaEstacionamento();
    this.estacionamento = this.estacionamentoService.estacionamento$;

    this.contarVagas();
  }

  contarVagas(){
    this.estacionamento.subscribe(v => {
      this.ocupadas = v.length;
      this.disponiveis = this.capacidade - this.ocupadas;
    });
  }
}
