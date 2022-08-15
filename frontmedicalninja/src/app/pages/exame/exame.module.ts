import { ExameService } from '../../@core/data/exame.service';
import { NgModule } from "@angular/core";
import { ThemeModule } from "../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";
import { ExameRoutingModule, routedComponents } from "./exame-routing.module";
import { WorklistService } from '../../@core/data/worklist.service';
import { PacienteService } from '../../@core/data/paciente.service';
import { PagerService } from '../../@core/data/pager.service';

@NgModule({
  imports: [ThemeModule, ExameRoutingModule, ToasterModule.forRoot()],
  declarations: [...routedComponents],
  providers: [ExameService, WorklistService, PacienteService, PagerService ]
})
export class ExameModule {}
