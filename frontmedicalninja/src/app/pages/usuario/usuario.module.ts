import { FacilityService } from './../../@core/data/facility.service';
import { EmpresaService } from './../../@core/data/empresa.service';

import { NgModule } from "@angular/core";

import { ThemeModule } from "../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";
import { UsuarioRoutingModule, routedComponents } from "./usuario-routing.module";
import { UsuarioService } from "../../@core/data/usuario.service";
import { PerfilService } from "../../@core/data/perfil.service";
import { NgSelectModule } from "@ng-select/ng-select";
import { NgxMaskModule } from "ngx-mask";

@NgModule({
  imports: [ThemeModule,
    UsuarioRoutingModule,
    ToasterModule.forRoot(),
    NgSelectModule,
    NgxMaskModule.forRoot(),],
  declarations: [...routedComponents],
  providers: [UsuarioService, PerfilService, EmpresaService, FacilityService]
})
export class UsuarioModule {}
