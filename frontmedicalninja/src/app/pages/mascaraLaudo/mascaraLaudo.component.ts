import { Component } from '@angular/core';
import { LogComponent } from '../../@core/model/log/logComponent';

@Component({
  moduleId: module.id,
  selector: 'mascaraLaudos',
  template: `<router-outlet></router-outlet>`,
})
export class MascaraLaudoComponent {
  Id: string;
  descricao: string;
  laudo: string;
  modalidade: string;
  status: boolean;
  log: LogComponent = new LogComponent();
}
