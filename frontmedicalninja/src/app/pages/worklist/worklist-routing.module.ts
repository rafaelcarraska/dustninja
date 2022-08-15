import { WorklistEstatisticaComponent } from './worklist-estatisticas/worklist-estatisticas.component';
import { WorklistConfirmacaoComponent } from './worklist-confirmacao/worklist-confirmacao.component';
import { WorklistAnexosComponent } from './worklist-anexos/worklist-anexos.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorklistComponent } from './worklist.component';
import { WorklistNotasComponent } from './worklist-notas/worklist-notas.component';
import { WorklistLogComponent } from './worklist-log/worklist-log.component';
import { WorklistReiterpretacaoComponent } from './worklist-reinterpretacao/worklist-reinterpretacao.component';
import { WorklistTemplateImpressaoComponent } from './worklist-templateImpressao/worklist-templateImpressao.component';
import { WorklistStatusComponent } from './worklist-status/worklist-status.component';
import { WorklistRoubarLaudoComponent } from './worklist-roubarLaudo/worklist-roubarLaudo.component';
import { WorklistDesmembrarComponent } from './worklist-desmembrar/worklist-desmembrar.component';
import { WorklistExamesAnterioresComponent } from './worklist-examesanteriores/worklist-examesanteriores.component';

const routes: Routes = [{
  path: '',
  component: WorklistComponent,
  children: [{
    path: 'worklist-notas',
    component: WorklistNotasComponent,
  },
  {
    path: 'worklist-anexos',
    component: WorklistAnexosComponent,
  },
  {
    path: 'worklist-comfirmacao',
    component: WorklistConfirmacaoComponent,
  },
  {
    path: 'worklist-reinterpretacao',
    component: WorklistReiterpretacaoComponent,
  },
  {
    path: 'worklist-templateImpressao',
    component: WorklistTemplateImpressaoComponent,
  },
  {
    path: 'worklist-log',
    component: WorklistLogComponent,
  },
  {
    path: 'worklist-status',
    component: WorklistStatusComponent,
  },
  {
    path: 'worklist-roubarLaudo',
    component: WorklistRoubarLaudoComponent,
  },
  {
    path: 'worklist-desmembrar',
    component: WorklistDesmembrarComponent,
  },
  {
    path: 'worklist-examesanteriores',
    component: WorklistExamesAnterioresComponent,
  },
  {
    path: 'worklist-estatisticas',
    component: WorklistEstatisticaComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorklistRoutingModule { }

export const routedComponents = [
  WorklistComponent,
  WorklistNotasComponent,
  WorklistAnexosComponent,
  WorklistLogComponent,
  WorklistConfirmacaoComponent,
  WorklistReiterpretacaoComponent,
  WorklistTemplateImpressaoComponent,
  WorklistStatusComponent,
  WorklistRoubarLaudoComponent,
  WorklistDesmembrarComponent,
  WorklistExamesAnterioresComponent,
  WorklistEstatisticaComponent
];
