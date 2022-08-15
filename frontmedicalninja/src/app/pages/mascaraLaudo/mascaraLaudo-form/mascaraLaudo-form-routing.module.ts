
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {MascaraLaudoFormComponent} from './mascaraLaudo-form.component';

const routes: Routes = [{
  path: '',
  component: MascaraLaudoFormComponent,
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MascaraLaudoFormRoutingModule { }

export const routedComponents = [
  MascaraLaudoFormComponent,
];
