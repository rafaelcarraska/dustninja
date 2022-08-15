
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TemplateImpressaoComponent } from './templateImpressao.component';
import { TemplateImpressaoGridComponent } from './templateImpressao-grid/templateImpressao-grid.component';
import { TemplateAnexosComponent } from './templateImpressao-anexos/templateImpressao-anexos.component';

const routes: Routes = [{
  path: '',
  component: TemplateImpressaoComponent,
  children: [{
    path: 'templateImpressao-grid',
    component: TemplateImpressaoGridComponent,
  },
  {
    path: 'templateImpressao-anexos',
    component: TemplateAnexosComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TemplateImpressaoRoutingModule { }

export const routedComponents = [
  TemplateImpressaoComponent,
  TemplateImpressaoGridComponent,
  TemplateAnexosComponent
];
