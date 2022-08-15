
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FiltrosComponent } from './filtros.component';
import { FiltrosGridComponent } from './filtros-grid/filtros-grid.component';

const routes: Routes = [{
  path: '',
  component: FiltrosComponent,
  children: [{
    path: 'filtros-grid',
    component: FiltrosGridComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FiltrosRoutingModule { }

export const routedComponents = [
  FiltrosComponent,
  FiltrosGridComponent,
];
