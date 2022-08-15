import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { AutenticacaoComponent } from '../model/autenticacao/autenticacao.component';


@Injectable()
export class LoginService {

  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + 'autenticacao/';

  constructor(http: HttpClient) {
    this.http = http;
    this.headers = new Headers();

    this.headers.append('Content-Type', 'application/json');
  }

  Sair(): Observable<any>{
    return this.http.get(this.url + `Logoff`, { responseType: "text" });
  }

  Logar(autenticacao: AutenticacaoComponent): Observable<any> {
    return this.http.put(this.url + `Autentica`, autenticacao);
  }

  SelecionaEmpresa(autenticacao: AutenticacaoComponent): Observable<any> {
    return this.http.put(this.url + `SelecionaEmpresa`, autenticacao);
  }

  DadosBasicos(): Observable<any> {
    return this.http.get(this.url + `DadosBasicos`);
  }
}
