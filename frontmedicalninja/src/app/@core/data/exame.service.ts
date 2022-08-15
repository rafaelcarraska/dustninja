import { FileDCMComponent } from './../model/fileDCM/fileDCM.component';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { BodyOutputType, Toast, ToasterService } from 'angular2-toaster';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { ConfirmacaoComponent } from '../model/fileDCM/confirmacao.component';
import { LogVisualizacaoComponent } from '../model/log/logVisualizacaoComponent';
import { DesmembrarComponent } from '../model/desmembrar/desmembrarComponenr';


@Injectable()
export class ExameService {

  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + 'exame/';

  constructor(http: HttpClient, private toasterService: ToasterService) {
    this.http = http;
    this.headers = new Headers();

    this.headers.append('Content-Type', 'application/json');
  }

  salva(fileDCM: FileDCMComponent): Observable<any> {
    return this.http.put(this.url + `Salva`, fileDCM);
  }

  HistoricoClinicoLoad(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `HistoricoClinicoLoad/${fileDCMId}`);
  }

  VerificaStatusExame(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `VerificaStatusExame/${fileDCMId}`);
  }

  LogVisualizacao(logVisualizacao: LogVisualizacaoComponent): Observable<any> {
    return this.http.put(this.url + `LogVisualizacao`, logVisualizacao);
  }

  ListaExameInfo(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `ListaExameInfo/${fileDCMId}`);
  }

  ListaExameInfoLaudado(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `ListaExameInfoLaudado/${fileDCMId}`);
  }

  VerificaRevisao(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `VerificaRevisao/${fileDCMId}`);
  }

  Confirmacao(confirmacao: ConfirmacaoComponent): Observable<any> {
    // console.log('confirmacao', confirmacao);
    return this.http.put(this.url + `Confirmacao`, confirmacao);
  }

  CarregaUltimoLaudo(fileDCMId: string): Observable<any> {
    return this.http.get(this.url + `CarregaUltimoLaudo/${fileDCMId}`);
  }

  Desmembrar(desmembrar: DesmembrarComponent): Observable<any> {
    return this.http.put(this.url + `Desmembrar`, desmembrar);
  }

  montaLogVisualizacao(tipo: string, fileDCMId: string){
    let logView = new LogVisualizacaoComponent();
    logView.tipo = tipo;
    logView.fileDCMId = fileDCMId;

    return logView;
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "Exame",
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
