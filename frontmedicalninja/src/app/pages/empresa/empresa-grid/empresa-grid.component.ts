import { Component } from "@angular/core";
import { EmpresaComponent } from "../empresa.component";
import { EmpresaService } from "../../../@core/data/empresa.service";
import { EmpresaModalComponent } from "../empresa-modal/empresa-modal.component";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ToasterConfig } from "angular2-toaster";

import "style-loader!angular2-toaster/toaster.css";

@Component({
  selector: "empresa-grid",
  templateUrl: "./empresa-grid.component.html",
  styleUrls: [
    "../../../@theme/styles/grid.scss",
    "./empresa-grid.component.scss"
  ]
})
export class EmpresaGridComponent {
  listaEmpresa: EmpresaComponent[] = [];
  empresaservice: EmpresaService;

  constructor(empresaservice: EmpresaService, private modalService: NgbModal) {
    this.empresaservice = empresaservice;

    this.LoadGrid();
  }
  config: ToasterConfig;

  LoadGrid() {
    this.empresaservice.lista().subscribe(
      listaEmpresa => {
        if (!listaEmpresa)
          this.empresaservice.showToast(
            "warning",
            "Nenhuma empresa localizada."
          );

        this.listaEmpresa = listaEmpresa;
      },
      erro => {
        this.empresaservice.erro(erro);
      }
    );
  }

  Novo() {
    const activeModal = this.modalService.open(EmpresaModalComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    let novaEmpresa = new EmpresaComponent();
    novaEmpresa.Id = "";
    novaEmpresa.status = true;
    activeModal.componentInstance.modalHeader = "Nova Empresa ";
    activeModal.componentInstance.empresa = novaEmpresa;
    activeModal.componentInstance.listaEmpresa = this.listaEmpresa;
  }

  Modal(empresa: EmpresaComponent) {
    const activeModal = this.modalService.open(EmpresaModalComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });

    activeModal.componentInstance.modalHeader =
      "Editar Empresa " + empresa.nomeFantasia;
    activeModal.componentInstance.empresa = empresa;
  }

  Deletar(empresa: EmpresaComponent): void {
    if (window.confirm("Deseja deletar essa Empresa?")) {
      this.empresaservice.remove(empresa).subscribe(
        msg => {
          if (!msg.erro) {
            let novaEmpresa = this.listaEmpresa.slice(0);
            let indice = novaEmpresa.indexOf(empresa);
            novaEmpresa.splice(indice, 1);
            this.listaEmpresa = novaEmpresa;
            this.empresaservice.showToast(
              "success",
              "Empresa deletada com sucesso."
            );
          } else {
            if (msg.status == false) {
              empresa.status = false;
            }
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.empresaservice.showToast("warning", "Aceso negado.");
          } else {
            this.empresaservice.showToast(
              "error",
              "Erro ao deletar o usuário."
            );
          }
        }
      );
    }
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.empresaservice.showToast("warning", erro);
      });
    }
  }
}
