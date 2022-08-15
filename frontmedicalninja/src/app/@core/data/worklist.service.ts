import { FileDCMNotaComponent } from './../model/NotaHistorico/fileDCMNota.component';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { BodyOutputType, Toast, ToasterService } from 'angular2-toaster';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { AnexoComponent } from '../model/anexos/anexos.component';
import { NotaComponent } from '../model/NotaHistorico/nota.component';
import { LaudoPageComponent } from '../model/laudoPage/laudoPage.component';

@Injectable()
export class WorklistService {
  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + 'worklist/';
  length: any;

  constructor(http: HttpClient, private toasterService: ToasterService) {

    this.http = http;
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');

  }

  GetLaudoPage(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `GetLaudoPage/${fileDCMId}`);
  }

  ServiceWindowsNinja(laudoPage: LaudoPageComponent): Observable<any> {
    let url = `https://pdf.fastpacs.com.br/ServiceWindowsNinja/pdf/getLaudoPDF`;

    return this.http.post(url, laudoPage, {
      responseType: "blob",
      headers: new HttpHeaders().append("Content-Type", "application/json")
    });
  }

  DownloadDICOM(url: string): Observable<any> {
    return this.http.get(url, {
      responseType: "blob",
      headers: new HttpHeaders().append("Content-Type", "application/json")
    });
  }

  lista(filtroPage: string): Observable<any> {
    if(filtroPage != '' && filtroPage != undefined){
      return this.http.get(this.url + `lista/${filtroPage}`);
    }else{
      return this.http.get(this.url + `lista`);
    }
  }

  listaFiltro(filtroId: string): Observable<any> {
    return this.http.get(this.url + `ListaFiltro/${filtroId}`);
  }

  listaExame(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `listaExame/${fileDCMId}`);
  }

  listaNotas(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `listaNotas/${fileDCMId}`);
  }

  DeletarNota(nota: NotaComponent): Observable<any> {
    return this.http.put(this.url + 'DeletarNota', nota);
  }

  listaeventos(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `listaEventos/${fileDCMId}`);
  }

  listaanexos(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `listaAnexos/${fileDCMId}`);
  }

  DeletarAnexo(anexo: AnexoComponent): Observable<any> {
    return this.http.delete(this.url + `DeletarAnexo/${anexo.Id}`);
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "worklist",
      body: body,
      timeout: 6000,
      showCloseButton: true,
      bodyOutputType: BodyOutputType.TrustedHtml
    };
    this.toasterService.popAsync(toast);
  }

  salvaNota(nota: FileDCMNotaComponent): Observable<any> {
    return this.http.put(this.url + `SalvaNota`, nota);
  }

   favoritar(fileDCMId: string): Observable<any> {
    return this.http.post(this.url + `Favoritar/${fileDCMId}`, fileDCMId);
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


