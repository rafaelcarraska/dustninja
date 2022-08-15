import { NgModule } from "@angular/core";
import { ThemeModule } from "../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";
import { FiltrosService } from "../../@core/data/filtros.service";
import { FiltrosRoutingModule, routedComponents } from "./filtros-routing.module";

@NgModule({
  imports: [ThemeModule, FiltrosRoutingModule, ToasterModule.forRoot()],
  declarations: [...routedComponents],
  providers: [FiltrosService]
})
export class FiltrosModule {}
