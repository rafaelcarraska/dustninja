import { Component } from "@angular/core";
import { FiltrosComponent } from "../filtros.component";
import { FiltrosService } from "../../../@core/data/filtros.service";
import {
  ToasterConfig
} from "angular2-toaster";

import "style-loader!angular2-toaster/toaster.css";
import { Router } from "@angular/router";

@Component({
  selector: "filtros-grid",
  templateUrl: "./filtros-grid.component.html",
  styleUrls: [
    "../../../@theme/styles/grid.scss",
    "./filtros-grid.component.scss"
  ]
})
export class FiltrosGridComponent {
  listaFiltros: FiltrosComponent[] = [];
  filtrosservice: FiltrosService;

  constructor(
    filtrosservice: FiltrosService,
    private readonly router: Router,
  ) {
    this.filtrosservice = filtrosservice;
    this.LoadGrid();
  }
  config: ToasterConfig;

  LoadGrid() {
    this.filtrosservice.lista().subscribe(
      listaFiltro => {
        if (!listaFiltro)
          this.filtrosservice.showToast("warning", "Nenhum filtro localizado.");

        this.listaFiltros = listaFiltro;
      },
      erro =>  {
        this.filtrosservice.erro(erro);
      }
    );
  }

  Novo() {
    this.router.navigate(["/pages/filtros/0"]);
  }

  Modal(filtros: FiltrosComponent) {
    this.router.navigate(["/pages/filtros/"+filtros.Id]);
  }

  Deletar(filtros): void {
    if (window.confirm("Deseja deletar esse filtro?")) {
      this.filtrosservice.remove(filtros).subscribe(
        msg => {
          if (!msg.erro) {
            let novoFiltros = this.listaFiltros.slice(0);
            let indice = novoFiltros.indexOf(filtros);
            novoFiltros.splice(indice, 1);
            this.listaFiltros = novoFiltros;
            this.filtrosservice.showToast("success", "Filtro deletado com sucesso.");
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.filtrosservice.showToast("warning", "Aceso negado.");
          }else{
            this.filtrosservice.showToast("error", "Erro ao deletar o filtro.");
          }
        }
      );
    }
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.filtrosservice.showToast("warning", erro);
      });
    }
  }



}
