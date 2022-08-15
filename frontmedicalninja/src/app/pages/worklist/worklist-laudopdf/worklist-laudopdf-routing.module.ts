import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LaudoPDFComponent } from './worklist-laudopdf.component';


const routes: Routes = [{
  path: '',
  component: LaudoPDFComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LaudoPDFRoutingModule { }

export const routedComponents = [
  LaudoPDFComponent
];
