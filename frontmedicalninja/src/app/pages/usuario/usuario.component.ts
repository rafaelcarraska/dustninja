
import { Component } from '@angular/core';
import { EnderecoComponent } from '../endereco/endereco.component';
import { ContatoComponent } from '../../@core/model/contato/contatoComponent';
import { AssinaturaComponent } from '../../@core/model/assinatura/assinaturaComponent';
import { LogComponent } from '../../@core/model/log/logComponent';

@Component({
  moduleId: module.id,
  selector: 'usuarios',
  template: `<router-outlet></router-outlet>`,
})
export class UsuarioComponent {
  Id: string;
  nome: string;
  login: string;
  email: string;
  perfilId: string;
  endereco: EnderecoComponent = new EnderecoComponent();
  master: boolean;
  status: boolean;
  listaContato: ContatoComponent[] = [];
  assinatura: AssinaturaComponent = new AssinaturaComponent();
  listaEmpresa: string[] = [];
  log: LogComponent = new LogComponent();
  twofactor: boolean;
}
