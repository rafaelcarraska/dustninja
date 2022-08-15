
import { NgModule } from '@angular/core';
import { ThemeModule } from '../../../@theme/theme.module';
import { WorklistService } from '../../../@core/data/worklist.service';
import { ToasterModule } from 'angular2-toaster';
import { PagerService } from '../../../@core/data/pager.service';
import { LaudoPDFRoutingModule, routedComponents } from './worklist-laudopdf-routing.module';
import { SafeHtmlPipe } from '../../../@theme/pipes/SafeHtmlPipe.pipe';

@NgModule({
  imports: [ ThemeModule, LaudoPDFRoutingModule, ToasterModule.forRoot()],
  declarations: [
    ...routedComponents, SafeHtmlPipe
  ],
  providers: [WorklistService, PagerService],
})
export class LaudoPDFModule { }


