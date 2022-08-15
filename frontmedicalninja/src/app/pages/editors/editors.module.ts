import { NgModule } from '@angular/core';

import { ThemeModule } from '../../@theme/theme.module';

import { EditorsRoutingModule, routedComponents } from './editors-routing.module';

@NgModule({
  imports: [
    ThemeModule,
    EditorsRoutingModule,
  ],
  declarations: [
    ...routedComponents,
  ],
})
export class EditorsModule { }
