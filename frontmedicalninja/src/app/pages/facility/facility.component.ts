import { Component } from '@angular/core';
import { EnderecoComponent } from '../endereco/endereco.component';
import { ContatoComponent } from '../../@core/model/contato/contatoComponent';
import { PrioridadeComponent } from '../../@core/model/Prioridade/prioridadeComponent';
import { LogComponent } from '../../@core/model/log/logComponent';

@Component({
  moduleId: module.id,
  selector: 'facility',
  template: `<router-outlet></router-outlet>`,
})
export class FacilityComponent {
  Id: string;
  descricao: string;
  razaoSocial: string;
  status: boolean;
  endereco: EnderecoComponent = new EnderecoComponent();
  listaContato: ContatoComponent[] = [];
  aeTitle: string;
  tempoRetencaoImagens: number;
  prioridade: PrioridadeComponent = new PrioridadeComponent();
  log: LogComponent = new LogComponent();
  notaRadiologista: string;
  listaTemplateImpressao: string[] = [];
}
