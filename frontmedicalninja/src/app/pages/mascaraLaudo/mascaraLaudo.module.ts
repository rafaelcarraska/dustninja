import { NgModule } from "@angular/core";
import { MascaraLaudoRoutingModule, routedComponents } from "./mascaraLaudo-routing.module";
import { MascaraLaudoService } from "../../@core/data/mascaraLaudo.service";
import { ThemeModule } from "../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";

@NgModule({
  imports: [ThemeModule, MascaraLaudoRoutingModule, ToasterModule.forRoot()],
  declarations: [...routedComponents],
  providers: [MascaraLaudoService]
})
export class MascaraLaudoModule {}
