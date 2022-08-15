import { Component } from "@angular/core";
import { TipoExameComponent } from "../tipoExame.component";
import { TipoExameService } from "../../../@core/data/tipoExame.service";
import { TipoExameModalComponent } from "../tipoExame-modal/tipoExame-modal.component";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ToasterConfig } from "angular2-toaster";
import "style-loader!angular2-toaster/toaster.css";

@Component({
  selector: "tipoExame-grid",
  templateUrl: "./tipoExame-grid.component.html",
  styleUrls: [
    "../../../@theme/styles/grid.scss",
    "./tipoExame-grid.component.scss"
  ]
})
export class TipoExameGridComponent {
  listaTipoExames: TipoExameComponent[] = [];
  tipoExameservice: TipoExameService; 

  constructor(tipoExameservice: TipoExameService, private modalService: NgbModal) {
    this.tipoExameservice = tipoExameservice;
    this.LoadGrid();
  }
  config: ToasterConfig;

  LoadGrid() {
    this.tipoExameservice.lista().subscribe(
      listaTipoExames => {
        if (!listaTipoExames)
          this.tipoExameservice.showToast("warning", "Nenhum TipoExame localizado.");

        this.listaTipoExames = listaTipoExames;
      },
      erro => {
        this.tipoExameservice.erro(erro);
      }
    );
  }

  Novo() {
    const activeModal = this.modalService.open(TipoExameModalComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    let novoTipoExame = new TipoExameComponent();
    novoTipoExame.Id = "";
    novoTipoExame.status = true;
    activeModal.componentInstance.modalHeader = "Novo TipoExame";
    activeModal.componentInstance.tipoExame = novoTipoExame;
    activeModal.componentInstance.listaTipoExames = this.listaTipoExames;
  }

  Modal(tipoExame: TipoExameComponent) {
    const activeModal = this.modalService.open(TipoExameModalComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });

    activeModal.componentInstance.modalHeader =
      "Editar TipoExame " + tipoExame.nome;
    activeModal.componentInstance.tipoExame = tipoExame;
  }

  Deletar(tipoExame): void {
    if (window.confirm("Deseja deletar esse TipoExame?")) {
      this.tipoExameservice.remove(tipoExame).subscribe(
        msg => {
          if (!msg.erro) {
            let novoTipoExame = this.listaTipoExames.slice(0);
            let indice = novoTipoExame.indexOf(tipoExame);
            novoTipoExame.splice(indice, 1);
            this.listaTipoExames = novoTipoExame;
            this.tipoExameservice.showToast(
              "success",
              "TipoExame deletado com sucesso."
            );
          } else {
            if(msg.status == false){
              tipoExame.status = false;
            }
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.tipoExameservice.showToast("warning", "Aceso negado.");
          }else{
            this.tipoExameservice.showToast("error", "Erro ao deletar o tipoExame.");
          }
        }
      );
    }
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.tipoExameservice.showToast("warning", erro);
      });
    }
  }
}
