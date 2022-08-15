
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PacienteComponent } from './paciente.component';
import { PacienteGridComponent } from './paciente-grid/paciente-grid.component';

const routes: Routes = [{
  path: '',
  component: PacienteComponent,
  children: [{
    path: 'paciente-grid',
    component: PacienteGridComponent
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PacienteRoutingModule { }

export const routedComponents = [
  PacienteComponent,
  PacienteGridComponent
];
