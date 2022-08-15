
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MascaraLaudoComponent } from './mascaraLaudo.component';
import { MascaraLaudoGridComponent } from './mascaraLaudo-grid/mascaraLaudo-grid.component';

const routes: Routes = [{
  path: '',
  component: MascaraLaudoComponent,
  children: [{
    path: 'mascaraLaudo-grid',
    component: MascaraLaudoGridComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MascaraLaudoRoutingModule { }

export const routedComponents = [
  MascaraLaudoComponent,
  MascaraLaudoGridComponent,
];
