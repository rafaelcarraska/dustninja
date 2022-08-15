
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {TemplateImpressaoFormComponent} from './templateImpressao-form.component';

const routes: Routes = [{
  path: '',
  component: TemplateImpressaoFormComponent,
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TemplateImpressaoFormRoutingModule { }

export const routedComponents = [
  TemplateImpressaoFormComponent,
];
