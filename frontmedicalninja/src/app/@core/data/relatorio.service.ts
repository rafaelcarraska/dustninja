import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { BodyOutputType, Toast, ToasterService } from 'angular2-toaster';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { RelatorioCSVComponent } from '../model/Relatorio/relatoriocsvComponent';

@Injectable()
export class RelatorioService {
  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + 'relatorio/';

  constructor(http: HttpClient, private toasterService: ToasterService) {
    this.http = http;
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');

  }
  
  RelatorioGerencial(relatoriocsv: RelatorioCSVComponent): Observable<any> {
    return this.http.post(this.url + "getRelatorioGerencial", relatoriocsv, {
      responseType: "blob",
      headers: new HttpHeaders().append("Content-Type", "application/json")
    });
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "relatorio",
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


