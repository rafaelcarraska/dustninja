
import { NgModule } from '@angular/core';
import { LaudoRoutingModule, routedComponents } from './worklist-laudo-routing.module';
import { ThemeModule } from '../../../@theme/theme.module';
import { WorklistService } from '../../../@core/data/worklist.service';
import { CKEditorModule } from 'ngx-ckeditor';
import { ToasterModule } from 'angular2-toaster';
import { MascaraLaudoService } from '../../../@core/data/mascaraLaudo.service';
import { PagerService } from '../../../@core/data/pager.service';
import { ExameService } from '../../../@core/data/exame.service';
import { TipoExameService } from '../../../@core/data/tipoExame.service';

@NgModule({
  imports: [ ThemeModule, LaudoRoutingModule, CKEditorModule, ToasterModule.forRoot()],
  declarations: [
    ...routedComponents,
  ],
  providers: [WorklistService, MascaraLaudoService, PagerService, ExameService, TipoExameService],
})
export class LaudoModule { }


