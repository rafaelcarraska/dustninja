import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Headers } from "@angular/http";
import { BodyOutputType, Toast, ToasterService } from "angular2-toaster";
import { Observable } from "rxjs";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/do";
import "rxjs/add/operator/map";
import { environment } from "../../../environments/environment";
import { EmpresaComponent } from "../../pages/empresa/empresa.component";

@Injectable()
export class EmpresaService {
  public empresaVM: EmpresaComponent = new EmpresaComponent();
  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + "empresa/";

  constructor(http: HttpClient, private toasterService: ToasterService) {
    this.http = http;
    this.headers = new Headers();
    this.headers.append("Content-Type", "application/json");
  }

  listaCombo(): Observable<any> {
    return this.http.get(this.url + "ListaCombo");
  }

  lista(): Observable<any> {
    return this.http.get(this.url + "Lista");
  }

  //TODO: buscar apenas as Telas de acessso da empresa
  listaEmpresa(): Observable<any> {
    return this.http.get(this.url + `EmpresaLogada`);
  }

  salva(empresa: EmpresaComponent): Observable<any> {
    return this.http.put(this.url + `Salva`, empresa);
  }

  remove(empresa: EmpresaComponent): Observable<any> {
    return this.http.delete(this.url + `Deleta/${empresa.Id}`);
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "Empresa",
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
