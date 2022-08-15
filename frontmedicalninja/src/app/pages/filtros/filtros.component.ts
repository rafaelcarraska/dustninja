import { Component } from '@angular/core';
import { LogComponent } from '../../@core/model/log/logComponent';

@Component({
  moduleId: module.id,
  selector: 'filtros',
  template: `<router-outlet></router-outlet>`,
})
export class FiltrosComponent {
  Id: string;
  descricao: string;
  status: boolean;
  particular: boolean;
  log: LogComponent = new LogComponent();
  listaFiltroStatus: FiltroStatusComponent[] = [];
  listaFiltroGerais: FiltroGeraisComponent[] = [];
  listaFiltroDatas: FiltroDatasComponent[] = [];
  listaFiltroOrdem: FiltroOrdemComponent[] = [];
  listaPerfil: FiltroPerfilComponent[] = [];
}

export class FiltroStatusComponent {
  Id: string;
  descricao: string;
}

export class FiltroDatasComponent {
  Id: string;
  descricao: string;
  dias: number;
}

export class FiltroGeraisComponent {
  Id: string;
  descricao: string;
  valor: string;
}

export class FiltroOrdemComponent {
  Id: string;
  descricao: string;
  ordem: string;
  prioridade: number;
}

export class FiltroPerfilComponent {
  Id: string;
  descricao: string;
}
