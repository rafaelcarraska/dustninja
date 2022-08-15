import { UsuarioService } from './../../../@core/data/usuario.service';
import { PerfilService } from './../../../@core/data/perfil.service';
import { ComboboxComponent } from './../../../@core/model/combobox/combobox.component';
import { Component } from "@angular/core";
import { FacilityComponent } from "../facility.component";
import { FacilityService } from "../../../@core/data/facility.service";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ToasterConfig } from 'angular2-toaster';
import { FacilityModalComponent } from '../facility-modal/facility-modal.component';
import { FacilityPermissaoComponent } from '../facility-permissao/facility-permissao.component';
import { TemplateImpressaoService } from '../../../@core/data/templateImpressao.service';

@Component({
  selector: "facility-grid",
  templateUrl: "./facility-grid.component.html",
  styleUrls: ['../../../@theme/styles/grid.scss', './facility-grid.component.scss']
})
export class FacilityGridComponent {
  listaFacilitys: FacilityComponent[] = [];
  listaComboPerfil: ComboboxComponent[] = [];
  facilityservice: FacilityService;
  perfilservice: PerfilService;
  usuarioservice: UsuarioService;
  listaUsuario: ComboboxComponent[] = [];
  listaTemplateImpressao: ComboboxComponent[] = [];
  templateImpressaoService: TemplateImpressaoService;
  saida = {
    value: "teste"
  }; 

  constructor(
    facilityservice: FacilityService,
    private modalService: NgbModal,
    perfilservice: PerfilService,
    usuarioservice: UsuarioService,
    templateImpressaoService: TemplateImpressaoService,
  ) {
    this.facilityservice = facilityservice;
    this.usuarioservice = usuarioservice;
    this.perfilservice = perfilservice;
    this.templateImpressaoService = templateImpressaoService;

    this.LoadGrid();
    this.LoadComboPerfil();
    this.LoadUsuario();
    this.LoadTemplateImpressao();
  }

  config: ToasterConfig;

  LoadUsuario(){
    this.usuarioservice.listaCombo().subscribe(
      listaUsuario => {
        if (!listaUsuario)
          console.log("Nenhum usuário localizado.");

        this.listaUsuario = listaUsuario;
      },
      erro =>  {
        this.usuarioservice.erro(erro);
      }
    );
  }

  LoadTemplateImpressao() {
    this.templateImpressaoService.listaCombo().subscribe(
      listaTemplateImpressao => {
        if (!listaTemplateImpressao){
          this.facilityservice.showToast(
            "error",
            "Nenhum template de impressão localizado."
          );
        }else{
          this.listaTemplateImpressao = listaTemplateImpressao;
        }
      },
      erro => {
        this.templateImpressaoService.erro(erro);
      }
    );
  }

  LoadGrid(){
    this.facilityservice.lista().subscribe(
      listaFacilitys => {
        if (!listaFacilitys)
        this.facilityservice.showToast("warning", "Nenhum Usuário localizado.");

        this.listaFacilitys = listaFacilitys;
      },
      erro => {
        this.facilityservice.erro(erro);
      }
    );
  }

  LoadComboPerfil() {
    this.perfilservice.listaCombo().subscribe(
      listaComboPerfil => {
        if (!listaComboPerfil)
          this.perfilservice.showToast("warning", "Nenhuma Perfil localizado.");

        this.listaComboPerfil = listaComboPerfil;
      },
      erro =>  {
        this.perfilservice.erro(erro);
      }
    );
  }

  Novo(){
    const activeModal = this.modalService.open(FacilityModalComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    let novaFacility = new FacilityComponent();
    novaFacility.Id = "";
    novaFacility.tempoRetencaoImagens = 30;
    novaFacility.status = true;
    activeModal.componentInstance.modalHeader = "Nova unidade";
    activeModal.componentInstance.facility = novaFacility;
    activeModal.componentInstance.listaFacilitys = this.listaFacilitys;
    activeModal.componentInstance.listaComboPerfil = this.listaComboPerfil;
    activeModal.componentInstance.listaUsuario = this.listaUsuario;
    activeModal.componentInstance.listaTemplateImpressao = this.listaTemplateImpressao;
  }

  Modal(facility: FacilityComponent) {
    const activeModal = this.modalService.open(FacilityModalComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });

    activeModal.componentInstance.modalHeader =
      "Editar unidade " + facility.descricao;
    activeModal.componentInstance.facility = facility;
    activeModal.componentInstance.listaFacilitys = this.listaFacilitys;
    activeModal.componentInstance.listaComboPerfil = this.listaComboPerfil;
    activeModal.componentInstance.listaUsuario = this.listaUsuario;
    activeModal.componentInstance.listaTemplateImpressao = this.listaTemplateImpressao;
  }

  Permissoes(facility: FacilityComponent){
    const activeModal = this.modalService.open(FacilityPermissaoComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });

    activeModal.componentInstance.modalHeader =
      "Editar Permissões da unidade " + facility.descricao;
    activeModal.componentInstance.facility = facility;
  }

  Deletar(facility): void {
    if (window.confirm("Deseja deletar a unidade "+ facility.descricao +"?")) {
      this.facilityservice.remove(facility.Id).subscribe(
        msg => {
          if (!msg.erro) {
            let novaFacility = this.listaFacilitys.slice(0);
            let indice = novaFacility.indexOf(facility);
            novaFacility.splice(indice, 1);
            this.listaFacilitys = novaFacility;
            this.facilityservice.showToast(
              "success",
              "Unidade deletada com sucesso."
            );
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.facilityservice.showToast("warning", "Aceso negado.");
          }else{
            this.facilityservice.showToast("error", "Erro ao deletar a Unidade.");
          }
        }
      );
    }
  }

  ShowErrors(errors) {
    console.log(errors);
    if (errors) {
      errors.forEach(erro => {
        this.facilityservice.showToast("warning", erro);
      });
    }
  }

}
