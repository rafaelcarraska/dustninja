import { Component } from "@angular/core";
import { PerfilComponent } from "../perfil.component";
import { PerfilService } from "../../../@core/data/perfil.service";
import { PerfilModalComponent } from "../perfil-modal/perfil-modal.component";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ToasterConfig } from "angular2-toaster";

import "style-loader!angular2-toaster/toaster.css";

@Component({
  selector: "perfil-grid",
  templateUrl: "./perfil-grid.component.html",
  styleUrls: [
    "../../../@theme/styles/grid.scss",
    "./perfil-grid.component.scss"
  ]
})
export class PerfilGridComponent {
  listaPerfis: PerfilComponent[] = [];
  perfilservice: PerfilService;

  constructor(perfilservice: PerfilService, private modalService: NgbModal) {
    this.perfilservice = perfilservice;
    this.LoadGrid();
  }
  config: ToasterConfig;

  LoadGrid() {
    this.perfilservice.lista().subscribe(
      listaPerfis => {
        if (!listaPerfis)
          this.perfilservice.showToast("warning", "Nenhuma Perfil localizada.");

        this.listaPerfis = listaPerfis;
      },
      erro => {
        this.perfilservice.erro(erro);
      }
    );
  }

  Novo() {
    const activeModal = this.modalService.open(PerfilModalComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    let novoPerfil = new PerfilComponent();
    novoPerfil.Id = "";
    novoPerfil.status = true;
    activeModal.componentInstance.modalHeader = "Novo Perfil";
    activeModal.componentInstance.perfil = novoPerfil;
    activeModal.componentInstance.listaPerfis = this.listaPerfis;
  }

  Modal(perfil: PerfilComponent) {
    const activeModal = this.modalService.open(PerfilModalComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });

    activeModal.componentInstance.modalHeader =
      "Editar Perfil " + perfil.descricao;
    activeModal.componentInstance.perfil = perfil;
  }

  Deletar(perfil): void {
    if (window.confirm("Deseja deletar esse Perfil?")) {
      this.perfilservice.remove(perfil).subscribe(
        msg => {
          if (!msg.erro) {
            let novoPerfil = this.listaPerfis.slice(0);
            let indice = novoPerfil.indexOf(perfil);
            novoPerfil.splice(indice, 1);
            this.listaPerfis = novoPerfil;
            this.perfilservice.showToast(
              "success",
              "Perfil deletado com sucesso."
            );
          } else {
            if(msg.status == false){
              perfil.status = false;
            }
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.perfilservice.showToast("warning", "Aceso negado.");
          }else{
            this.perfilservice.showToast("error", "Erro ao deletar o perfil.");
          }
        }
      );
    }
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.perfilservice.showToast("warning", erro);
      });
    }
  }
}
