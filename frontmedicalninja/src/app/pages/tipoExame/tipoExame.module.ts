
import { NgModule } from "@angular/core";
import { TreeModule } from 'angular-tree-component';
import { TipoExameRoutingModule, routedComponents } from "./tipoExame-routing.module";
import { TipoExameService } from "../../@core/data/tipoExame.service";
import { ThemeModule } from "../../@theme/theme.module";
import { ToasterModule } from "angular2-toaster";

@NgModule({
  imports: [ThemeModule, TipoExameRoutingModule, TreeModule, ToasterModule.forRoot()],
  declarations: [...routedComponents],
  providers: [TipoExameService]
})
export class TipoExameModule {}
