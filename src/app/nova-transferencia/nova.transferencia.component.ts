import { Component } from '@angular/core';

@Component({
  selector: 'app-nova-transferencia',
  templateUrl: './nova-transferencia.component.html',
  styleUrls: ['./nova.transferencia.component.scss'],
})


export class NovaTransferenciaComponent {

  valor:number | undefined;
  destino:number | undefined;


  transferir() {
    console.log('Solicitada nova transferncia');
    console.log('Solicitada nova transferncia');
    console.log('valor:', this.valor);
    console.log('destino:', this.destino);
  }
}
