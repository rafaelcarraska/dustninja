import { APP_BASE_HREF } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ApplicationModule, ErrorHandler, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { ToasterModule } from "angular2-toaster";
import { HttpModule } from '../../node_modules/@angular/http';
import { CoreModule } from './@core/core.module';
import { ThemeModule } from './@theme/theme.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CustomErrorHandler } from './app.errorhandler';
import { AuthenticationGuard } from './app.guard';
import { CustomHttpInterceptor } from './app.interceptor';
import { LoginModule } from './pages/login/login.module';
import {NgxMaskModule} from 'ngx-mask'
import { CKEditorModule } from 'ngx-ckeditor';
import { LaudoModule } from './pages/worklist/Worklist-laudo/worklist-laudo.module';
import { LaudoPDFModule } from './pages/worklist/worklist-laudopdf/worklist-laudopdf.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    ApplicationModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    NgSelectModule,
    LoginModule,
    CKEditorModule,
    FormsModule,
    LaudoModule,
    LaudoPDFModule,
    NgbModule.forRoot(),
    ThemeModule.forRoot(),
    CoreModule.forRoot(),
    ToasterModule.forRoot(),
    NgxMaskModule.forRoot()

  ],
  bootstrap: [AppComponent],
  providers: [
    { provide: APP_BASE_HREF, useValue: '/' },
    { provide: ErrorHandler, useClass: CustomErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: CustomHttpInterceptor, multi: true },
    AuthenticationGuard
  ],
})
export class AppModule {
}
