import { NgModule } from "@angular/core";
import { MascaraLaudoService } from "../../../@core/data/mascaraLaudo.service";
import { ThemeModule } from "../../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";
import { MascaraLaudoFormRoutingModule, routedComponents } from "./mascaraLaudo-form-routing.module";
import { CKEditorModule } from "ngx-ckeditor";

@NgModule({
  imports: [ThemeModule, MascaraLaudoFormRoutingModule, ToasterModule.forRoot(), CKEditorModule,],
  declarations: [...routedComponents],
  providers: [MascaraLaudoService]
})
export class MascaraLaudoFormModule {}
