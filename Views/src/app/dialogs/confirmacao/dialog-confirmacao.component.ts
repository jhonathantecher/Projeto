import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Data } from '@angular/router';

@Component({
  selector: 'app-dialog-confirmacao',
  templateUrl: './dialog-confirmacao.component.html',
  styleUrls: ['./dialog-confirmacao.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DialogConfirmacaoComponent{

  constructor(public dialogRef: MatDialogRef<DialogConfirmacaoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Data) {}

    confirmar(){
      this.dialogRef.close(true)
    }
}
