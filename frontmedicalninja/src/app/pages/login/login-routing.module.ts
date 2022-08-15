
import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';
import { LoginComponent } from './login.component';


const routes: Routes = [{
  path: '',
  component: LoginComponent,
}];

@NgModule({

})
export class LoginRoutingModule { }

export const routedComponents = [
  LoginComponent,
];
