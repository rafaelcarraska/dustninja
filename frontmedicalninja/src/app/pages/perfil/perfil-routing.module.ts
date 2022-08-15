
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PerfilComponent } from './perfil.component';
import { PerfilGridComponent } from './perfil-grid/perfil-grid.component';
import { PerfilModalComponent } from './perfil-modal/perfil-modal.component';

const routes: Routes = [{
  path: '',
  component: PerfilComponent,
  children: [{
    path: 'perfil-grid',
    component: PerfilGridComponent,
  }, {
    path: 'modals',
    component: PerfilModalComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PerfilRoutingModule { }

export const routedComponents = [
  PerfilComponent,
  PerfilGridComponent,
  PerfilModalComponent,
];
