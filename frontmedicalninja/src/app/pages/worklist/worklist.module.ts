import { TemplateImpressaoService } from './../../@core/data/templateImpressao.service';
import { FacilityService } from './../../@core/data/facility.service';
import { ExameService } from './../../@core/data/exame.service';
import { NgModule } from '@angular/core';
import { ThemeModule } from '../../@theme/theme.module';
import { WorklistRoutingModule, routedComponents } from './worklist-routing.module';
import { WorklistService } from '../../@core/data/worklist.service';
import { ToasterModule } from "angular2-toaster";
import { FilterWorkListPipe } from '../../@theme/pipes/filterWorkList';
import { PagerService } from '../../@core/data/pager.service';
import { ConfiguracaoService } from '../../@core/data/configuracao.service';
import { LightboxModule } from 'ngx-lightbox';
import { ContextMenuModule } from 'ngx-contextmenu';
import { NgxEchartsModule } from 'ngx-echarts';

@NgModule({
  imports: [ThemeModule, WorklistRoutingModule, ToasterModule.forRoot(), LightboxModule, ContextMenuModule.forRoot(), NgxEchartsModule ],
  declarations: [...routedComponents, FilterWorkListPipe],
  providers: [WorklistService, PagerService, ConfiguracaoService, ExameService, FacilityService, TemplateImpressaoService],
})
export class WorklistModule { }

