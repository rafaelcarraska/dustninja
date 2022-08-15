import { Component } from "@angular/core";
import {
  ToasterConfig
} from "angular2-toaster";

import "style-loader!angular2-toaster/toaster.css";
import { Router } from "@angular/router";
import { RelatorioService } from "../../../@core/data/relatorio.service";
import { saveAs } from 'file-saver';
import { RelatorioCSVComponent } from "../../../@core/model/Relatorio/relatoriocsvComponent";
import { load } from "@angular/core/src/render3/instructions";

@Component({
  selector: "relatorio-gerencial",
  templateUrl: "./relatorio-gerencial.component.html",
  styleUrls: [
    "../../../@theme/styles/grid.scss",
    "./relatorio-gerencial.component.scss"
  ]
})
export class RelatorioGerencialComponent {
  relatorioservice: RelatorioService;
  load: boolean = false;
  relatoriocsv: RelatorioCSVComponent = new RelatorioCSVComponent();

  constructor(
    relatorioservice: RelatorioService,
    private readonly router: Router,
  ) {
    this.relatorioservice = relatorioservice;
    
  }
  config: ToasterConfig;

  ngOnInit() {
    let today = new Date();
    this.relatoriocsv.de = today.getFullYear() + '-' + ('0' + (today.getMonth() + 1)).slice(-2) + '-' + ('0' + today.getDate()).slice(-2);
    this.relatoriocsv.ate = today.getFullYear() + '-' + ('0' + (today.getMonth() + 1)).slice(-2) + '-' + ('0' + today.getDate()).slice(-2);
    this.relatoriocsv.separador = ",";
  }

  Download(){
    this.load = true;
    let filename = "/relatorioGerencial.csv";
    this.relatorioservice.RelatorioGerencial(this.relatoriocsv).subscribe(
      data => {
        saveAs(data, filename);
      },
      err => {
        alert("Erro ao realizar o download do arquivo.");
        console.error(err);
      },
      () => {
        this.load = false;
      }
    );
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.relatorioservice.showToast("warning", erro);
      });
    }
  }



}
