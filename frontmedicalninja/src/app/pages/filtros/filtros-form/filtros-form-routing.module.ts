
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FiltrosFormComponent } from './filtros-form.component';

const routes: Routes = [{
  path: '',
  component: FiltrosFormComponent,
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FiltrosFormRoutingModule { }

export const routedComponents = [
    FiltrosFormComponent,
];
