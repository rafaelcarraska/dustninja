import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { BodyOutputType, Toast, ToasterService } from 'angular2-toaster';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { MascaraLaudoComponent } from '../../pages/mascaraLaudo/mascaraLaudo.component';

@Injectable()
export class MascaraLaudoService {

  public mascaraLaudoVM: MascaraLaudoComponent = new MascaraLaudoComponent();
  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + 'mascaraLaudo/';

  constructor(http: HttpClient, private toasterService: ToasterService) {

    this.http = http;
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');
  }

  lista(): Observable<any> {
    return this.http.get(this.url + "lista");
  }

  listaCombo(): Observable<any> {
    return this.http.get(this.url + "ListaCombo");
  }

  listaOrderbyModalidade(Id: string): Observable<any> {
    return this.http.get(this.url + `listaOrderbyModalidade/${Id}`);
  }

  ListaMascaraLaudo(Id: string): Observable<any> {
    return this.http.get(this.url + `ListaMascaraLaudo/${Id}`);
  }

  salva(mascaraLaudo: MascaraLaudoComponent): Observable<any> {
    return this.http.put(this.url + `Salva`, mascaraLaudo);
  }

  remove(mascaraLaudo: MascaraLaudoComponent): Observable<any> {
    return this.http.delete(this.url + `Deleta/${mascaraLaudo.Id}`);
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "MascaraLaudo",
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


