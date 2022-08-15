import { PerfilService } from './../../@core/data/perfil.service';
import { NgModule } from "@angular/core";
import { NgSelectModule } from "@ng-select/ng-select";
import { NgxMaskModule } from "ngx-mask";
import { ThemeModule } from "../../@theme/theme.module";
import { FacilityRoutingModule, routedComponents } from "./facility-routing.module";
import { FacilityService } from "../../@core/data/facility.service";
import { ToasterModule } from "angular2-toaster";
import { UsuarioService } from '../../@core/data/usuario.service';
import { TemplateImpressaoService } from '../../@core/data/templateImpressao.service';

@NgModule({
  imports: [ThemeModule,
    FacilityRoutingModule,
    ToasterModule.forRoot(),
    NgSelectModule,
    NgxMaskModule.forRoot(),],
  declarations: [...routedComponents],
  providers: [FacilityService, PerfilService, UsuarioService, TemplateImpressaoService]
})
export class FacilityModule {}
