import { NgModule } from "@angular/core";
import { FiltrosService } from "../../../@core/data/filtros.service";
import { ThemeModule } from "../../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";
import { FiltrosFormRoutingModule, routedComponents } from "./filtros-form-routing.module";

@NgModule({
  imports: [ThemeModule, FiltrosFormRoutingModule, ToasterModule.forRoot()],
  declarations: [...routedComponents],
  providers: [FiltrosService]
})
export class FiltrosFormModule {}
