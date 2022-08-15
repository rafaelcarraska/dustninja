import { NgModule } from "@angular/core";
import { ThemeModule } from "../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";
import { RelatorioRoutingModule, routedComponents } from "./gerencial-routing.module";
import { RelatorioService } from "../../@core/data/relatorio.service";

@NgModule({
  imports: [ThemeModule, RelatorioRoutingModule, ToasterModule.forRoot()],
  declarations: [...routedComponents],
  providers: [RelatorioService]
})
export class RelatorioModule {}
