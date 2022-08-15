import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FacilityComponent } from './facility.component';
import { FacilityGridComponent } from './facility-grid/facility-grid.component';
import { FacilityModalComponent } from './facility-modal/facility-modal.component';
import { FacilityPermissaoComponent } from './facility-permissao/facility-permissao.component';

const routes: Routes = [{
  path: '',
  component: FacilityComponent,
  children: [{
    path: 'facility-grid',
    component: FacilityGridComponent,
  }, {
    path: 'modals',
    component: FacilityModalComponent,
  },{
    path: 'permissao',
    component: FacilityPermissaoComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FacilityRoutingModule { }

export const routedComponents = [
  FacilityComponent,
  FacilityGridComponent,
  FacilityModalComponent,
  FacilityPermissaoComponent,
];
