import { NgModule } from "@angular/core";
import { PacienteService } from "../../../@core/data/paciente.service";
import { ThemeModule } from "../../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";
import { PacienteFormRoutingModule, routedComponents } from "./paciente-form-routing.module";

@NgModule({
  imports: [ThemeModule, PacienteFormRoutingModule, ToasterModule.forRoot()],
  declarations: [...routedComponents],
  providers: [PacienteService]
})
export class PacienteFormModule {}
