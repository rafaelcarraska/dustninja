
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RelatorioComponent } from './relatorio.component';
import { RelatorioGerencialComponent } from './gerencial/relatorio-gerencial.component';

const routes: Routes = [{
  path: '',
  component: RelatorioComponent,
  children: [{
    path: 'relatorio-gerencial',
    component: RelatorioGerencialComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RelatorioRoutingModule { }

export const routedComponents = [
    RelatorioComponent,
    RelatorioGerencialComponent,
];
