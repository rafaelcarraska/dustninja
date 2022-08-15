import { NgModule } from "@angular/core";
import { PacienteRoutingModule, routedComponents } from "./paciente-routing.module";
import { PacienteService } from "../../@core/data/paciente.service";
import { ThemeModule } from "../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";

@NgModule({
  imports: [ThemeModule, PacienteRoutingModule, ToasterModule.forRoot()],
  declarations: [...routedComponents],
  providers: [PacienteService]
})
export class PacienteModule {}
