import { ExameModule } from './exame/exame.module';
import { PacienteFormModule } from './paciente/paciente-form/paciente-form.module';
import { PacienteModule } from './paciente/paciente.module';
import { LaudoPDFModule } from './worklist/worklist-laudopdf/worklist-laudopdf.module';
import { LaudoModule } from './worklist/Worklist-laudo/worklist-laudo.module';
import { TemplateImpressaoModule } from './templateImpressao/templateImpressao.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ThemeModule } from '../@theme/theme.module';
import { CustomHttpInterceptor } from '../app.interceptor';
import { ContatoComponent } from './../@core/model/contato/contatoComponent';
import { EmpresaModule } from './empresa/empresa.module';
import { EnderecoModule } from './endereco/endereco.module';
import { FacilityModule } from './facility/facility.module';
import { MascaraLaudoModule } from './mascaraLaudo/mascaraLaudo.module';
import { MiscellaneousModule } from './miscellaneous/miscellaneous.module';
import { PagesRoutingModule } from './pages-routing.module';
import { PagesComponent } from './pages.component';
import { PerfilModule } from './perfil/perfil.module';
import { UsuarioModule } from './usuario/usuario.module';
import { WorklistModule } from './worklist/worklist.module';
import { MascaraLaudoFormModule } from './mascaraLaudo/mascaraLaudo-form/mascaraLaudo-form.module';
import { TipoExameModule } from './tipoExame/tipoExame.module';
import { FiltrosModule } from './filtros/filtros.module';
import { TemplateImpressaoFormModule } from './templateImpressao/templateImpressao-form/templateImpressao-form.module';
import { FiltrosFormModule } from './filtros/filtros-form/filtros-form.module';
import { RelatorioModule } from './relatorio/relatorio.module';


const PAGES_COMPONENTS = [
  PagesComponent,
];

@NgModule({
  imports: [
    PagesRoutingModule,
    ThemeModule,
    WorklistModule,
    MiscellaneousModule,
    FacilityModule,
    EmpresaModule,
    UsuarioModule,
    EnderecoModule,
    PerfilModule,
    MascaraLaudoModule,
    MascaraLaudoFormModule,
    TemplateImpressaoModule,
    TemplateImpressaoFormModule,
    FiltrosModule,
    FiltrosFormModule,
    RelatorioModule,
    TipoExameModule,
    LaudoModule,
    LaudoPDFModule,
    PacienteModule,
    PacienteFormModule,
    ExameModule
  ],
  declarations: [
    ...PAGES_COMPONENTS,
  ],
  providers: [ContatoComponent,
    { provide: HTTP_INTERCEPTORS, useClass: CustomHttpInterceptor, multi: true }]
})
export class PagesModule {
}
