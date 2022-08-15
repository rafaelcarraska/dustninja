import { Component } from "@angular/core";
import { TemplateImpressaoComponent } from "../templateImpressao.component";
import { TemplateImpressaoService } from "../../../@core/data/templateImpressao.service";
import {
  ToasterConfig
} from "angular2-toaster";

import "style-loader!angular2-toaster/toaster.css";
import { Router } from "@angular/router";

@Component({
  selector: "templateImpressao-grid",
  templateUrl: "./templateImpressao-grid.component.html",
  styleUrls: [
    "../../../@theme/styles/grid.scss",
    "./templateImpressao-grid.component.scss"
  ]
})
export class TemplateImpressaoGridComponent {
  listaTemplate: TemplateImpressaoComponent[] = [];
  templateImpressaoservice: TemplateImpressaoService;

  constructor(
    templateImpressaoservice: TemplateImpressaoService,
    private readonly router: Router,
  ) {
    this.templateImpressaoservice = templateImpressaoservice;
    this.LoadGrid();
  }
  config: ToasterConfig;

  LoadGrid() {
    this.templateImpressaoservice.lista().subscribe(
      listaTemplate => {
        if (!listaTemplate)
          this.templateImpressaoservice.showToast("warning", "Nenhum template de impressão localizada.");

        this.listaTemplate = listaTemplate;
      },
      erro =>  {
        this.templateImpressaoservice.erro(erro);
      }
    );
  }

  Novo() {
    this.router.navigate(["/pages/templateImpressao/0"]);
  }

  Modal(templateImpressao: TemplateImpressaoComponent) {
    this.router.navigate(["/pages/templateImpressao/"+templateImpressao.Id]);
  }

  Deletar(templateImpressao): void {
    if (window.confirm("Deseja deletar esse template de impressão?")) {
      this.templateImpressaoservice.remove(templateImpressao).subscribe(
        msg => {
          if (!msg.erro) {
            let novoTemplateImpressao = this.listaTemplate.slice(0);
            let indice = novoTemplateImpressao.indexOf(templateImpressao);
            novoTemplateImpressao.splice(indice, 1);
            this.listaTemplate = novoTemplateImpressao;
            this.templateImpressaoservice.showToast("success", "Template de impressão deletado com sucesso.");
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.templateImpressaoservice.showToast("warning", "Aceso negado.");
          }else{
            this.templateImpressaoservice.showToast("error", "Erro ao deletar o template de impressão.");
          }
        }
      );
    }
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.templateImpressaoservice.showToast("warning", erro);
      });
    }
  }



}
