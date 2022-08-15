import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { BodyOutputType, Toast, ToasterService } from 'angular2-toaster';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { TemplateImpressaoComponent } from '../../pages/templateImpressao/templateImpressao.component';
import { AnexoTemplateComponent } from '../model/anexosTemplate/anexosTemplate.component';

@Injectable()
export class TemplateImpressaoService {

  public templateImpressaoLaudoVM: TemplateImpressaoComponent = new TemplateImpressaoComponent();
  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + 'templateImpressao/';

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

  ListaComboByFacility(Id: string): Observable<any> {
    return this.http.get(this.url + `ListaComboByFacility/${Id}`);
  }

  ListaTemplateImpressao(Id: string): Observable<any> {
    return this.http.get(this.url + `ListaTemplateImpressao/${Id}`);
  }

  TemplateImpresasaoUtilizado(Id: string): Observable<any> {
    return this.http.get(this.url + `TemplateImpresasaoUtilizado/${Id}`);
  }

  salva(templateImpressao: TemplateImpressaoComponent): Observable<any> {
    return this.http.put(this.url + `Salva`, templateImpressao);
  }

  remove(templateImpressao: TemplateImpressaoComponent): Observable<any> {
    return this.http.delete(this.url + `Deleta/${templateImpressao.Id}`);
  }

  listaAnexos(): Observable<any> {
    return this.http.get(this.url + `listaAnexos`);
  }

  DeletarAnexo(anexo: AnexoTemplateComponent): Observable<any> {
    return this.http.delete(this.url + `DeletarAnexo/${anexo.Id}`);
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "templateImpressao",
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


