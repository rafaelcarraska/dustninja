import { Component, Input, Output } from '@angular/core';

@Component({
  moduleId: module.id,
  selector: 'endereco',
  template: `<router-outlet></router-outlet>`,
})

export class EnderecoComponent {
  endereco: string;
  numero: string;
  bairro: string;
  complemento: string;
  cep: string;
  cidade: string;
  uf: string;
  obs: string;
}
