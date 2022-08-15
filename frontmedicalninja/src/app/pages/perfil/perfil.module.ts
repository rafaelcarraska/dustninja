
import { NgModule } from "@angular/core";
import { TreeModule } from 'angular-tree-component';
import { PerfilRoutingModule, routedComponents } from "./perfil-routing.module";
import { PerfilService } from "../../@core/data/perfil.service";
import { ThemeModule } from "../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";

@NgModule({
  imports: [ThemeModule, PerfilRoutingModule, TreeModule, ToasterModule.forRoot()],
  declarations: [...routedComponents],
  providers: [PerfilService]
})
export class PerfilModule {}
