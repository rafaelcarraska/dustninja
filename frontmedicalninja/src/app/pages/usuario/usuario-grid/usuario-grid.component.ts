import { EmpresaService } from './../../../@core/data/empresa.service';
import { PerfilService } from './../../../@core/data/perfil.service';
import { Component } from "@angular/core";
import { UsuarioComponent } from "../usuario.component";
import { UsuarioService } from "../../../@core/data/usuario.service";
import { UsuarioModalComponent } from "../usuario-modal/usuario-modal.component";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ComboboxComponent } from './../../../@core/model/combobox/combobox.component';
import {
  ToasterConfig
} from "angular2-toaster";
import { Router } from '@angular/router';
import { UsuarioPermissaoComponent } from '../usuario-permissao/usuario-permissao.component';
import { FacilityService } from '../../../@core/data/facility.service';
import { UsuarioTwoFactorComponent } from '../usuario-twofactor/usuario-twofactor.component';

@Component({
  selector: "usuario-grid",
  templateUrl: "./usuario-grid.component.html",
  styleUrls: [
    "../../../@theme/styles/grid.scss",
    "./usuario-grid.component.scss",
  ]
})
export class UsuarioGridComponent {
  [x: string]: any;
  listaUsuarios: UsuarioComponent[] = [];
  listaComboPerfil: ComboboxComponent[] = [];
  listaEmpresa: ComboboxComponent[] = [];
  usuarioservice: UsuarioService;
  facilityservice: FacilityService;
  perfilservice: PerfilService;
  empresaservice: EmpresaService;

  constructor(
    facilityservice: FacilityService,
    usuarioservice: UsuarioService,
    perfilservice: PerfilService,
    empresaservice: EmpresaService,
    private modalService: NgbModal,
    private readonly router: Router,) {
      this.facilityservice = facilityservice;
      this.usuarioservice = usuarioservice;
      this.perfilservice = perfilservice;
      this.empresaservice = empresaservice;

      this.LoadGrid();
      this.LoadComboPerfil();
      this.LoadEmpresa();
  }
  config: ToasterConfig;



  LoadComboPerfil() {
    this.perfilservice.listaCombo().subscribe(
      listaComboPerfil => {
        if (!listaComboPerfil)
          this.perfilservice.showToast("warning", "Nenhuma Perfil localizado.");

        this.listaComboPerfil = listaComboPerfil;
      },
      erro => {
        this.perfilservice.erro(erro);
      }
    );
  }

  LoadEmpresa(){
    this.empresaservice.listaCombo().subscribe(
      listaEmpresa => {
        if (!listaEmpresa)
          console.log("Nenhuma empresa localizada.");

        this.listaEmpresa = listaEmpresa;
        this.autenticacao.empresaId = this.listaEmpresa[0].Id;
      },
      erro =>  {
        this.empresaservice.erro(erro);
      }
    );
  }

  LoadGrid() {
    this.usuarioservice.lista().subscribe(
      listaUsuarios => {
        if (!listaUsuarios)
          this.usuarioservice.showToast("warning", "Nenhum Usuário localizado.");

        this.listaUsuarios = listaUsuarios;
      },
      erro =>  {
        this.usuarioservice.erro(erro);
      }
    );
  }

  Novo() {
    const activeModal = this.modalService.open(UsuarioModalComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    let novoUsuario = new UsuarioComponent();
    novoUsuario.Id = "";
    novoUsuario.status = true;
    activeModal.componentInstance.modalHeader = "Novo Usuário";
    activeModal.componentInstance.usuario = novoUsuario;
    activeModal.componentInstance.listaUsuarios = this.listaUsuarios;
    activeModal.componentInstance.listaComboPerfil = this.listaComboPerfil;
    activeModal.componentInstance.listaEmpresa = this.listaEmpresa;

    // this.router.navigate(["/pages/usuario/"]);
  }

  Modal(usuario: UsuarioComponent) {
    const activeModal = this.modalService.open(UsuarioModalComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });

    activeModal.componentInstance.modalHeader =
      "Editar Usuário " + usuario.nome;
    activeModal.componentInstance.usuario = usuario;
    activeModal.componentInstance.listaUsuarios = this.listaUsuarios;
    activeModal.componentInstance.listaComboPerfil = this.listaComboPerfil;
    activeModal.componentInstance.listaEmpresa = this.listaEmpresa;

    // this.router.navigate(["/pages/usuario/"+usuario.Id]);
  }

  Permissoes(usuario: UsuarioComponent){
    const activeModal = this.modalService.open(UsuarioPermissaoComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });

    activeModal.componentInstance.modalHeader =
      "Editar Permissões do usuário " + usuario.nome;
    activeModal.componentInstance.usuario = usuario;
  }

  Twofactor(usuario: UsuarioComponent){
    const activeModal = this.modalService.open(UsuarioTwoFactorComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });

    activeModal.componentInstance.modalHeader =
      "Autenticação em dois fatores: " + usuario.nome;
    activeModal.componentInstance.usuario = usuario;
  }

  Deletar(usuario): void {
    if (window.confirm("Deseja deletar esse Usuário?")) {
      this.usuarioservice.remove(usuario).subscribe(
        msg => {
          if (!msg.erro) {
            let novoUsuario = this.listaUsuarios.slice(0);
            let indice = novoUsuario.indexOf(usuario);
            novoUsuario.splice(indice, 1);
            this.listaUsuarios = novoUsuario;
            this.usuarioservice.showToast(
              "success",
              "Usuário deletado com sucesso."
            );
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.usuarioservice.showToast("warning", "Aceso negado.");
          }else{
            this.usuarioservice.showToast("error", "Erro ao deletar o usuário.");
          }
        }
      );
    }
  }

  ShowErrors(errors) {
    console.log(errors);
    if (errors) {
      errors.forEach(erro => {
        this.usuarioservice.showToast("warning", erro);
      });
    }
  }
}
