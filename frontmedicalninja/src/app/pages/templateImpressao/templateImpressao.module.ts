import { NgModule } from "@angular/core";
import { TemplateImpressaoRoutingModule, routedComponents } from "./templateImpressao-routing.module";
import { TemplateImpressaoService } from "../../@core/data/templateImpressao.service";
import { ThemeModule } from "../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";

@NgModule({
  imports: [ThemeModule, TemplateImpressaoRoutingModule, ToasterModule.forRoot()],
  declarations: [...routedComponents],
  providers: [TemplateImpressaoService]
})
export class TemplateImpressaoModule {}
