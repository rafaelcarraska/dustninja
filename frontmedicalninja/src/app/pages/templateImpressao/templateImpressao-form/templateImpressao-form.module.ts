import { NgModule } from "@angular/core";
import { TemplateImpressaoService } from "../../../@core/data/templateImpressao.service";
import { ThemeModule } from "../../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";
import { TemplateImpressaoFormRoutingModule, routedComponents } from "./templateImpressao-form-routing.module";
import { CKEditorModule } from "ngx-ckeditor";

@NgModule({
  imports: [ThemeModule, TemplateImpressaoFormRoutingModule, ToasterModule.forRoot(), CKEditorModule,],
  declarations: [...routedComponents],
  providers: [TemplateImpressaoService]
})
export class TemplateImpressaoFormModule {}
