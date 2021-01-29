import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class GerarTicketFormService {
  form: FormGroup
  disableForm = true;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      veiculo: this.construirForm(),
    });
  }

  construirForm() {
    return new FormGroup({
      placa: new FormControl(null, [Validators.required, Validators.minLength(7)]),
      marca: new FormControl(null, Validators.required),
      modelo: new FormControl(null, Validators.required),
      nome: new FormControl(null, Validators.required),
    });
  }

  getErrorMessage(campo: string) {
    if (campo == 'placa') {
      if (this.form.get('veiculo').get('placa').hasError('required')) {
        return 'Placa é obrigatória.';
      }
      if (this.form.get('veiculo').get('placa').hasError('minlength')) {
        return 'Placa Inválida.';
      }
    }

    if (campo == 'marca'){
      if (this.form.get('veiculo').get('marca').hasError('required')) {
        return 'Marca é obrigatória.';
      }
    }

    if (campo == 'modelo'){
      if (this.form.get('veiculo').get('modelo').hasError('required')) {
        return 'Modelo é obrigatório.';
      }
    }

    if (campo == 'nome'){
      if (this.form.get('veiculo').get('nome').hasError('required')) {
        return 'Nome é obrigatório.';
      }
    }
  }
}
