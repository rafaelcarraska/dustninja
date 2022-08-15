import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UsuarioComponent } from './usuario.component';
import { UsuarioGridComponent } from './usuario-grid/usuario-grid.component';
import { UsuarioModalComponent } from './usuario-modal/usuario-modal.component';
import { UsuarioPermissaoComponent } from './usuario-permissao/usuario-permissao.component';
import { UsuarioTwoFactorComponent } from './usuario-twofactor/usuario-twofactor.component';

const routes: Routes = [{
  path: '',
  component: UsuarioComponent,
  children: [{
    path: 'usuario-grid',
    component: UsuarioGridComponent,
  }, {
    path: 'modals',
    component: UsuarioModalComponent,
  },{
    path: 'permissao',
    component: UsuarioPermissaoComponent,
  },{
    path: 'twofactor',
    component: UsuarioTwoFactorComponent,
  }]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UsuarioRoutingModule { }

export const routedComponents = [
  UsuarioComponent,
  UsuarioGridComponent,
  UsuarioModalComponent,
  UsuarioPermissaoComponent,
  UsuarioTwoFactorComponent
];
