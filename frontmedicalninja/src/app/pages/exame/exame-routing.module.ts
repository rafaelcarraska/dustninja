
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ExameComponent } from './Exame.component';
import { ExamePacienteComponent } from './exame-paciente/exame-paciente.component';

const routes: Routes = [{
  path: '',
  component: ExameComponent,children: [{
    path: 'exame-paciente',
    component: ExamePacienteComponent,
  }]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ExameRoutingModule { }

export const routedComponents = [
  ExameComponent,
  ExamePacienteComponent
];
