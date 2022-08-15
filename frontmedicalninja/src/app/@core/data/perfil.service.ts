import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Headers } from "@angular/http";
import { BodyOutputType, Toast, ToasterService } from "angular2-toaster";
import { Observable } from "rxjs";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/do";
import "rxjs/add/operator/map";
import { environment } from "../../../environments/environment";
import { PerfilComponent } from "../../pages/perfil/perfil.component";

@Injectable()
export class PerfilService {
  public perfilVM: PerfilComponent = new PerfilComponent();
  http: HttpClient;
  headers: Headers;
  url: string = environment.serviceUrl + "perfil/";

  constructor(http: HttpClient, private toasterService: ToasterService) {
    this.http = http;
    this.headers = new Headers();
    this.headers.append("Content-Type", "application/json");
  }

  lista(): Observable<any> {
    return this.http.get(this.url + "Lista");
  }

  listaCombo(): Observable<any> {
    return this.http.get(this.url + "ListaCombo");
  }

  salva(perfil: PerfilComponent): Observable<any> {
    return this.http.put(this.url + `Salva`, perfil);
  }

  remove(perfil: PerfilComponent): Observable<any> {
    return this.http.delete(this.url + `Deleta/${perfil.Id}`);
  }

  showToast(type: string, body: string) {
    const toast: Toast = {
      type: type,
      title: "Perfil",
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
