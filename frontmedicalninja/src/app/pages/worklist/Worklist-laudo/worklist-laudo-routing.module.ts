import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LaudoComponent } from './worklist-laudo.component';
import { TipoEstudoComponent } from './tipoEstudo/tipoEstudo.component';
import { HistoricoClinicoComponent } from './historicoClinico/historicoClinico.component';
import { RevisorComponent } from './revisor/revisor.component';


const routes: Routes = [{
  path: '',
  component: LaudoComponent,
  children: [{
    path: 'tipoEstudo',
    component: TipoEstudoComponent,
  },
  {
    path: 'historicoClinico',
    component: HistoricoClinicoComponent,
  },
  {
    path: 'revisor',
    component: RevisorComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LaudoRoutingModule { }

export const routedComponents = [
  LaudoComponent,
  TipoEstudoComponent,
  HistoricoClinicoComponent,
  RevisorComponent
];
