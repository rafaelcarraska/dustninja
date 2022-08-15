import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Headers } from '@angular/http';
import { BodyOutputType, Toast, ToasterService } from 'angular2-toaster';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment';
import { PacienteComponent } from '../../pages/paciente/paciente.component';
import { TrocaPacienteComponent } from '../model/fileDCM/trocaPaciente.component';

@Injectable()
export class PacienteService {

  publicpacienteLaudoVM: PacienteComponent = new PacienteComponent();
  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + 'paciente/';

  constructor(http: HttpClient, private toasterService: ToasterService) {

    this.http = http;
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');
  }

  lista(): Observable<any> {
    return this.http.get(this.url + "lista");
  }

  ListaFacility(facilityId: string): Observable<any> {
    return this.http.get(this.url + `ListaFacility/${facilityId}`);
  }

  listaCombo(): Observable<any> {
    return this.http.get(this.url + "ListaCombo");
  }

  ListaComboByFacility(Id: string): Observable<any> {
    return this.http.get(this.url + `ListaComboByFacility/${Id}`);
  }

  ListaPaciente(Id: string): Observable<any> {
    return this.http.get(this.url + `ListaPaciente/${Id}`);
  }

  PacienteUtilizado(Id: string): Observable<any> {
    return this.http.get(this.url + `PacienteUtilizado/${Id}`);
  }

  salva(paciente: PacienteComponent): Observable<any> {
    return this.http.put(this.url + `Salva`, paciente);
  }

  TrocaPaciente(trocaPaciente: TrocaPacienteComponent): Observable<any> {
    return this.http.put(this.url + `TrocaPaciente`, trocaPaciente);
  }

  remove(paciente: PacienteComponent): Observable<any> {
    return this.http.delete(this.url + `Deleta/${paciente.Id}`);
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "paciente",
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


