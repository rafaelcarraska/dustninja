import { LogComponent } from './../../@core/model/log/logComponent';
import { AcessoComponent } from './../../@core/model/acesso/acesso.component';
import { Component } from '@angular/core';

@Component({
  moduleId: module.id,
  selector: 'perfil',
  template: `<router-outlet></router-outlet>`,
})
export class PerfilComponent {
  Id: string;
  descricao: string;
  status: boolean;
  acesso: AcessoComponent = new AcessoComponent();
  log: LogComponent = new LogComponent();
}
