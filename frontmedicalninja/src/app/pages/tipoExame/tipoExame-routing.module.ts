
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TipoExameComponent } from './tipoExame.component';
import { TipoExameGridComponent } from './tipoExame-grid/tipoExame-grid.component';
import { TipoExameModalComponent } from './tipoExame-modal/tipoExame-modal.component';

const routes: Routes = [{
  path: '',
  component: TipoExameComponent,
  children: [{
    path: 'tipoExame-grid',
    component: TipoExameGridComponent,
  }, {
    path: 'modals',
    component: TipoExameModalComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TipoExameRoutingModule { }

export const routedComponents = [
  TipoExameComponent,
  TipoExameGridComponent,
  TipoExameModalComponent,
];
