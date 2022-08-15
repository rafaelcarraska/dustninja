import { LogComponent } from './../../@core/model/log/logComponent';
import { Component } from '@angular/core';

@Component({
  moduleId: module.id,
  selector: 'tipoExame',
  template: `<router-outlet></router-outlet>`,
})
export class TipoExameComponent {
  Id: string;
  nome: string;
  modalidade: string;
  codCobranca: string;
  palavraChave: string;
  mascaraLaudoId: string;
  status: boolean;
  log: LogComponent = new LogComponent();
}
