import { NgModule } from "@angular/core";

import { ThemeModule } from "../../@theme/theme.module";
import { EnderecoRoutingModule, routedComponents } from "./endereco-routing.component";
import { EnderecoService } from "../../@core/data/endereco.service";

@NgModule({
  imports: [ThemeModule, EnderecoRoutingModule],
  declarations: [...routedComponents],
  providers: [EnderecoService]
})
export class EnderecoModule {}
