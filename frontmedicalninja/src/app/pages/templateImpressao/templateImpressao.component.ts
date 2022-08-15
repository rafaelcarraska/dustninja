import { Component } from '@angular/core';
import { LogComponent } from '../../@core/model/log/logComponent';

@Component({
  moduleId: module.id,
  selector: 'templateImpressoes',
  template: `<router-outlet></router-outlet>`,
})
export class TemplateImpressaoComponent {
  Id: string;
  descricao: string;
  cabecalho: string;
  corpo: string;
  rodape: string;
  status: boolean;
  repetiCabecalho: boolean;
  repetiRodape: boolean;
  headerHeight: number;
  footerHeight: number;
  countAnexo: number;
  log: LogComponent = new LogComponent();
}
