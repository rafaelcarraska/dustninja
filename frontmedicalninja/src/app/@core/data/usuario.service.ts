import { ValidaTwoFactorComponent } from './../model/autenticacao/validaTwoFactor.component';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { BodyOutputType, Toast, ToasterService } from 'angular2-toaster';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { UsuarioComponent } from '../../pages/usuario/usuario.component';
import { UsuarioSenhaComponent } from './../model/usuariosenha/usuariosenhaComponent';
import { PermissaoUsuarioComponent } from '../model/permissaoUsuario/permissaoUsuario.component';
import { batchPermissoesComponent } from '../model/batchPermissoes/batchPermissoes.component';


@Injectable()
export class UsuarioService {

  public usuarioVM: UsuarioComponent = new UsuarioComponent();
  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + 'usuario/';
  usuarioSenha: UsuarioSenhaComponent;

  constructor(http: HttpClient, private toasterService: ToasterService) {

    this.http = http;
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');
    this.usuarioSenha = new UsuarioSenhaComponent();
  }

  listaCombo(): Observable<any> {
    return this.http.get(this.url + "ListaCombo");
  }

  ListaPermissao(usuarioId: string): Observable<any> {
    return this.http
      .get(this.url + `ListaPermissao/${usuarioId}`);
  }

  lista(): Observable<any> {
    return this.http.get(this.url + "lista");
  }

  listaUsuario(Id: string): Observable<any> {
    return this.http.get(this.url + `listaUsuario/${Id}`);
  }

  ListaTwoFactor(Id: string): Observable<any> {
    return this.http.get(this.url + `ListaTwoFactor/${Id}`);
  }

  salva(usuario: UsuarioComponent, senha: string): Observable<any> {
    this.usuarioSenha.usuario = usuario;
    this.usuarioSenha.senha = senha;

    return this.http.put(this.url + `Salva`, this.usuarioSenha);
  }

  NovoTwoFactor(Id: string): Observable<any> {
    return this.http.get(this.url + `NovoTwoFactor/${Id}`);
  }

  ValidaTwoFactor(validaTwoFactor: ValidaTwoFactorComponent): Observable<any> {
    return this.http.put(this.url + `ValidaTwoFactor`, validaTwoFactor);
  }

  ProcessarBatch(batchPermissoesUsuario: batchPermissoesComponent): Observable<any> {
    return this.http.put(this.url + `ProcessarBatch`, batchPermissoesUsuario);
  }

  SalvaPermissao(permissaoUsuario: PermissaoUsuarioComponent): Observable<any> {
    return this.http.put(this.url + `SalvaPermissao`, permissaoUsuario);
  }

  remove(usuario: UsuarioComponent): Observable<any> {
    return this.http.delete(this.url + `Deleta/${usuario.Id}`);
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "Usuário",
      body: body,
      timeout: 6000,
      showCloseButton: true,
      bodyOutputType: BodyOutputType.TrustedHtml
    };
    this.toasterService.popAsync(toast);
  }

  erro(erro) {
    switch (erro.status) {
      case 400:
        this.showToast("error", erro);
        break;
      case 401:
        this.showToast("error", "Aceso negado.");
        break;
      case 403:
        this.showToast("error", "Aceso negado.");
        break;
      default:
        this.showToast(
          "error",
          "Não foi possivel realizar o processo, entre em contato com o administrador."
        );
        break;
    }
  }

}


