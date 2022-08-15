import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { BodyOutputType, Toast, ToasterService } from 'angular2-toaster';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { TipoExameComponent } from '../../pages/tipoExame/tipoExame.component';

@Injectable()
export class TipoExameService {

  public tipoExameVM: TipoExameComponent = new TipoExameComponent();
  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + 'tipoExame/';

  constructor(http: HttpClient, private toasterService: ToasterService) {

    this.http = http;
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');
  }

  lista(): Observable<any> {
    return this.http.get(this.url + "lista");
  }

  listaCombo(): Observable<any> {
    return this.http.get(this.url + "listaCombo");
  }

  listaTipoEstudo(modalidade: string): Observable<any> {
    return this.http.get(this.url + `listaTipoEstudo/${modalidade}`);
  }

  ListaTipoExamel(Id: string): Observable<any> {
    return this.http.get(this.url + `ListaTipoExamel/${Id}`);
  }

  salva(tipoExame: TipoExameComponent): Observable<any> {
    return this.http.put(this.url + `Salva`, tipoExame);
  }

  remove(tipoExame: TipoExameComponent): Observable<any> {
    return this.http.delete(this.url + `Deleta/${tipoExame.Id}`);
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "TipoExame",
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


