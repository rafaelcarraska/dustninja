import { EmpresaService } from './../../@core/data/empresa.service';

import { NgModule } from '@angular/core';
import { LoginRoutingModule, routedComponents } from './login-routing.module';
import { ThemeModule } from '../../@theme/theme.module';
import { LoginService } from '../../@core/data/login.service';

@NgModule({
  imports: [
    ThemeModule,
    LoginRoutingModule,
  ],
  declarations: [
    ...routedComponents,
  ],
  providers: [LoginService, EmpresaService]
})
export class LoginModule { }


