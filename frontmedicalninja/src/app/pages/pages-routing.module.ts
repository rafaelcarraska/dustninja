import { LaudoComponent } from './worklist/Worklist-laudo/worklist-laudo.component';
import { TemplateImpressaoFormComponent } from './templateImpressao/templateImpressao-form/templateImpressao-form.component';
import { AuthenticationGuard } from './../app.guard';
import { WorklistComponent } from './worklist/worklist.component';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { PagesComponent } from './pages.component';
import { NotFoundComponent } from './miscellaneous/not-found/not-found.component';
import { MascaraLaudoFormComponent } from './mascaraLaudo/mascaraLaudo-form/mascaraLaudo-form.component';
import { FiltrosFormComponent } from './filtros/filtros-form/filtros-form.component';
import { PacienteFormComponent } from './paciente/paciente-form/paciente-form.component';
import { ExameComponent } from './exame/Exame.component';

const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [{
    path: 'worklist',
    component: WorklistComponent,
    canActivate: [AuthenticationGuard],
  },{
    path: 'worklist/:id',
    component: WorklistComponent,
    canActivate: [AuthenticationGuard],
  }, {
    path: 'exame/:id',
    loadChildren: './exame/exame.module#ExameModule',
    canActivate: [AuthenticationGuard],
  }, {
    path: 'facility',
    loadChildren: './facility/facility.module#FacilityModule',
    canActivate: [AuthenticationGuard],
  },{
    path: 'empresa',
    loadChildren: './empresa/empresa.module#EmpresaModule',
    canActivate: [AuthenticationGuard],
  },{
    path: 'usuarios',
    loadChildren: './usuario/usuario.module#UsuarioModule',
    canActivate: [AuthenticationGuard],
  }, {
    path: 'perfil',
    loadChildren: './perfil/perfil.module#PerfilModule',
    canActivate: [AuthenticationGuard],
  }, {
    path: 'mascaraLaudos',
    loadChildren: './mascaraLaudo/mascaraLaudo.module#MascaraLaudoModule',
    canActivate: [AuthenticationGuard],
  }, {
    path: 'mascaraLaudo/:id',
    component: MascaraLaudoFormComponent,
    canActivate: [AuthenticationGuard],
  },{
    path: 'laudo/:id',
    component: LaudoComponent,
    canActivate: [AuthenticationGuard],
  },{
    path: 'templateImpressoes',
    loadChildren: './templateImpressao/templateImpressao.module#TemplateImpressaoModule',
    canActivate: [AuthenticationGuard],
  }, {
    path: 'templateImpressao/:id',
    component: TemplateImpressaoFormComponent,
    canActivate: [AuthenticationGuard],
  },{
    path: 'paciente',
    loadChildren: './paciente/paciente.module#PacienteModule',
    canActivate: [AuthenticationGuard],
  }, {
    path: 'paciente/:id',
    component: PacienteFormComponent,
    canActivate: [AuthenticationGuard],
  },{
    path: 'filtros',
    loadChildren: './filtros/filtros.module#FiltrosModule',
    canActivate: [AuthenticationGuard],
  },{
    path: 'relatorio',
    loadChildren: './relatorio/relatorio.module#RelatorioModule',
    canActivate: [AuthenticationGuard],
  }, {
    path: 'filtros/:id',
    component: FiltrosFormComponent,
    canActivate: [AuthenticationGuard],
  }, {
    path: 'tipoExame',
    loadChildren: './tipoExame/tipoExame.module#TipoExameModule',
    canActivate: [AuthenticationGuard],
  },{
    path: '',
    redirectTo: 'worklist',
    pathMatch: 'full',
  }, {
    path: '**',
    component: NotFoundComponent,
  }],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
}
