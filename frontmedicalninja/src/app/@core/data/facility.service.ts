import { PermissaoFacilityComponent } from './../model/permissaoFalicity/permissaoFacility.component';

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { FacilityComponent } from '../../pages/facility/facility.component';
import { BodyOutputType, Toast, ToasterService } from 'angular2-toaster';
import { batchPermissoesComponent } from '../model/batchPermissoes/batchPermissoes.component';

@Injectable()
export class FacilityService {

  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + 'facility/';

  constructor(http: HttpClient, private toasterService: ToasterService) {

    this.http = http;
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');
  }

  lista(): Observable<any> {
    return this.http
      .get(this.url + "lista");
  }

  listaCombo(): Observable<any> {
    return this.http.get(this.url + "ListaCombo");
  }

  PrioridadeFacility(facilityId: string): Observable<any> {
    return this.http.get(this.url + `PrioridadeFacility/${facilityId}`);
  }

  ListaComboByUsuario(usuarioId: string): Observable<any> {
    return this.http.get(this.url +`ListaComboByUsuario/${usuarioId}`);
  }

  ListaPermissao(facilityId: string): Observable<any> {
    return this.http
      .get(this.url + `ListaPermissao/${facilityId}`);
  }

  salva(facility: FacilityComponent): Observable<any> {
    return this.http.put(this.url + `Salva`, facility);
  }

  ProcessarBatch(batchPermissoesFacility: batchPermissoesComponent): Observable<any> {
    return this.http.put(this.url + `ProcessarBatch`, batchPermissoesFacility);
  }

  SalvaPermissao(permissaoFacility: PermissaoFacilityComponent): Observable<any> {
    return this.http.put(this.url + `SalvaPermissao`, permissaoFacility);
  }

  remove(facilityId: string): Observable<any> {
    return this.http.delete(this.url + `Deleta/${facilityId}`);
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "Unidade",
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


