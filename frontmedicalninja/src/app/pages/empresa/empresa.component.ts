import { Component} from '@angular/core';
import { EnderecoComponent } from '../endereco/endereco.component';
import { TelaComponent } from '../../@core/model/tela/tela.component';
import { LogComponent } from '../../@core/model/log/logComponent';
import { ContatoComponent } from '../../@core/model/contato/contatoComponent';
import { AssinaturaComponent } from '../../@core/model/assinatura/assinaturaComponent';

@Component({
  moduleId: module.id,
  selector: 'empresa',
  template: `<router-outlet></router-outlet>`,
})

export class EmpresaComponent {
  Id: string;
  razaoSocial
  nomeFantasia: string;
  cnpj: string;
  responsavel: string;
  endereco: EnderecoComponent = new EnderecoComponent();
  status: boolean;
  listaTela: TelaComponent[];
  listaContato: ContatoComponent[] = [];
  assinatura: AssinaturaComponent;
  log: LogComponent = new LogComponent();
}
