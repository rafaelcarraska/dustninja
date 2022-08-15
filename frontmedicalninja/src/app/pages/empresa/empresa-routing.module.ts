import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmpresaComponent } from './empresa.component';
import { EmpresaGridComponent } from './empresa-grid/empresa-grid.component';
import { EmpresaModalComponent } from './empresa-modal/empresa-modal.component';

const routes: Routes = [{
  path: '',
  component: EmpresaComponent,
  children: [{
    path: 'empresa-grid',
    component: EmpresaGridComponent,
  }, {
    path: 'modals',
    component: EmpresaModalComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EmpresaRoutingModule { }

export const routedComponents = [
  EmpresaComponent,
  EmpresaGridComponent,
  EmpresaModalComponent,
];
