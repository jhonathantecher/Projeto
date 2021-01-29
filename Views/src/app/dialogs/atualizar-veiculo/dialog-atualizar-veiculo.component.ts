import { OnInit } from '@angular/core';
import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VeiculoModel } from 'src/app/veiculos/model/veiculo.model';
import { VeiculoService } from 'src/app/veiculos/veiculo.service';

@Component({
  selector: 'app-dialog-atualizar-veiculo',
  templateUrl: './dialog-atualizar-veiculo.component.html',
  styleUrls: ['./dialog-atualizar-veiculo.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DialogAtualizarVeiculoComponent implements OnInit {
  form: FormGroup;
  clienteId: number;
  veiculo: VeiculoModel;

  constructor(public dialogRef: MatDialogRef<DialogAtualizarVeiculoComponent>,
              @Inject(MAT_DIALOG_DATA) public data: VeiculoModel,
              private fb: FormBuilder,
              private veiculoService: VeiculoService){
    this.veiculo = data;
    this.form = this.fb.group({
      veiculo: this.construirForm(),
    });
  }

  ngOnInit() {
    this.form.get('veiculo').patchValue(this.veiculo);
  }

  construirForm() {
    return new FormGroup({
      placa: new FormControl({value: null, disabled: true}),
      marca: new FormControl(null, Validators.required),
      modelo: new FormControl(null, Validators.required),
      nome: new FormControl(null, Validators.required),
    });
  }

  getErrorMessage(campo: string) {
    if (campo == 'marca') {
      if (this.form.get('veiculo').get('marca').hasError('required')) {
        return 'Marca é obrigatória.';
      }
    }

    if (campo == 'modelo') {
      if (this.form.get('veiculo').get('modelo').hasError('required')) {
        return 'Modelo é obrigatório.';
      }
    }

    if (campo == 'nome') {
      if (this.form.get('veiculo').get('nome').hasError('required')) {
        return 'Nome é obrigatório.';
      }
    }
  }

  salvarVeiculo() {
    var veiculo: VeiculoModel = this.form.get('veiculo').value;
    veiculo.placa = this.veiculo.placa;
    veiculo.clienteId = this.veiculo.clienteId;

    this.veiculoService.atualizarVeiculo(veiculo);
  }
}
