import { NgModule } from "@angular/core";

import { ThemeModule } from "../../@theme/theme.module";
import { EmpresaRoutingModule, routedComponents } from "./empresa-routing.module";
import { EmpresaService } from "../../@core/data/empresa.service";
import { NgSelectModule } from "@ng-select/ng-select";
import { ToasterModule } from "angular2-toaster";
import { NgxMaskModule } from "ngx-mask";

@NgModule({
  imports: [ThemeModule, EmpresaRoutingModule, ToasterModule.forRoot(), NgSelectModule, NgxMaskModule.forRoot(),],
  declarations: [...routedComponents],
  providers: [EmpresaService]
})
export class EmpresaModule {}
