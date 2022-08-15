import { FacilityService } from "./../../../@core/data/facility.service";

import { ComboboxComponent } from "../../../@core/model/combobox/combobox.component";
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { UsuarioService } from "../../../@core/data/usuario.service";
import { FacilityComponent } from "../facility.component";
import { PermissaoComponent } from "../../../@core/model/permissao/permissao.component";
import { PermissaoFacilityComponent } from "../../../@core/model/permissaoFalicity/permissaoFacility.component";
import { batchPermissoesComponent } from "../../../@core/model/batchPermissoes/batchPermissoes.component";

@Component({
  moduleId: module.id,
  selector: "./facility-permissao",
  templateUrl: "./facility-permissao.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./facility-permissao.component.scss"
  ]
})
export class FacilityPermissaoComponent {
  @Input()
  usuarioservice: UsuarioService;
  facilityservice: FacilityService;

  facility: FacilityComponent = new FacilityComponent();
  modalHeader: string = "";

  listaUsuarioCombo: ComboboxComponent[];
  batchPermissoesFacility: batchPermissoesComponent = new batchPermissoesComponent();
  usuarioSelecionado: string;
  listaPermissoesModal: any;
  listaPermissoesModalBatch: any;
  batch: boolean;
  listaPermissoes: PermissaoComponent[] = [];
  listaUsuario: ComboboxComponent[];

  constructor(
    private activeModal: NgbActiveModal,
    usuarioservice: UsuarioService,
    facilityservice: FacilityService
  ) {
    this.usuarioservice = usuarioservice;
    this.facilityservice = facilityservice;
    this.usuarioSelecionado = "";
    this.batch = false;
    this.listaPermissoesModal = [
      {
        status: false,
        id: "Confirmar exame",
        descricao: "Confirmar exame"
      },
      {
        status: false,
        id: "Laudar imagem",
        descricao: "Laudar imagem"
      },
      {
        status: false,
        id: "Download da imagem",
        descricao: "Download da imagem"
      },
      {
        status: false,
        id: "Remover notas",
        descricao: "Remover notas"
      }
    ];

    this.listaPermissoesModalBatch = [
      {
        status: false,
        id: "Laudar imagem",
        descricao: "Laudar imagem"
      },
      {
        status: false,
        id: "Download da imagem",
        descricao: "Download da imagem"
      },
      {
        status: false,
        id: "Remover notas",
        descricao: "Remover notas"
      }
    ];
  }

  SelectBacth() {
    this.batch =
      this.listaPermissoesModalBatch.filter(x => x.status == true).length > 0;
  }

  ProcessarBatch() {
    if (window.confirm("Esse processo sobrescreve a configuração atual.")) {
      this.batchPermissoesFacility.facilityId = this.facility.Id;
      this.batchPermissoesFacility.listUsuarios = this.listaUsuario.filter(
        x => x.status == true
      );
      this.batchPermissoesFacility.listaPermissoes = this.listaPermissoesModalBatch.filter(
        x => x.status == true
      );

      this.facilityservice
        .ProcessarBatch(this.batchPermissoesFacility)
        .subscribe(
          msg => {
            if (!msg.erro) {
              this.facilityservice.showToast(
                "success",
                "batch processado com sucesso!"
              );
              this.closeModal();
            } else {
              this.ShowErrors(msg.erro);
            }
          },
          erro => {
            if (erro.status === 400) {
              this.InputErros(erro.error);
            }
            if (erro.status === 401 || erro.status === 403) {
              this.facilityservice.showToast("warning", "Aceso negado.");
            }
            this.facilityservice.showToast(
              "error",
              "Não foi possível processar o batch!"
            );
          }
        );
    }
  }

  ngAfterViewInit() {
    this.LoadUsuario();
  }

  checkAll(ev) {
    this.listaUsuario.forEach(x => {
      x.status = ev.target.checked;
      this.checkUsuario(x);
    });
  }

  LoadUsuario() {
    this.usuarioservice.listaCombo().subscribe(
      listaUsuario => {
        if (!listaUsuario) console.log("Nenhum usuário localizado.");

        this.listaUsuario = listaUsuario;
        this.CarregarPermissaoFacility();
      },
      erro => {
        this.usuarioservice.erro(erro);
      }
    );
  }

  CarregarPermissaoFacility() {
    if (this.facility.Id != "") {
      this.facilityservice.ListaPermissao(this.facility.Id).subscribe(
        permissao => {
          this.listaPermissoes = permissao;
          this.PermissaoFacility();
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.facilityservice.showToast("warning", "Aceso negado.");
          } else {
            this.facilityservice.showToast(
              "error",
              "Erro ao carregar permissoes."
            );
          }
        }
      );
    }
  }

  PermissaoFacility() {
    this.listaUsuario.forEach(x => {
      x.status =
        this.listaPermissoes.filter(p => p.usuarioId == x.Id).length > 0;
    });
    this.listaUsuarioCombo = this.listaUsuario.filter(x => x.status == true);
  }

  CarregarPermissoes(usuarioSelecionado: string) {
    let permissoes = this.listaPermissoes.filter(
      p => p.facilityId == this.facility.Id && p.usuarioId == usuarioSelecionado
    )[0].listaPermissao;

    this.listaPermissoesModal.forEach(x => {
      x.status = permissoes.filter(p => p == x.descricao).length > 0;
      // this.checkPermissao(x);

    });
  }

  checkPermissao(permissao) {
    let lista = this.listaPermissoes.filter(
      x =>
        x.usuarioId == this.usuarioSelecionado &&
        x.facilityId == this.facility.Id
    )[0];
    if (permissao.status) {
      if (lista.listaPermissao.filter(p => p == permissao.id).length == 0) {
        lista.listaPermissao.push(permissao.id);
      }
    } else {
      let indice = lista.listaPermissao.indexOf(
        lista.listaPermissao.filter(x => x == permissao.id)[0]
      );
      lista.listaPermissao.splice(indice, 1);
    }
  }

  checkUsuario(usuario: ComboboxComponent) {
    let permisssao = new PermissaoComponent();
    if (usuario.status) {
      permisssao.facilityId = this.facility.Id;
      permisssao.usuarioId = usuario.Id;
      permisssao.listaPermissao = [];
      this.listaPermissoes.push(permisssao);
    } else {
      let indice = this.listaPermissoes.indexOf(
        this.listaPermissoes.filter(x => x.usuarioId == usuario.Id)[0]
      );
      this.listaPermissoes.splice(indice, 1);
    }

    this.listaUsuarioCombo = this.listaUsuario.filter(x => x.status == true);
  }

  onChangeUsuario(value: string) {
    if (value != "0") {
      this.listaPermissoesModal.forEach(element => {
        element.status = false;
      });
      this.usuarioSelecionado = value;
      this.CarregarPermissoes(this.usuarioSelecionado);
    }
  }

  closeModal() {
    this.activeModal.close();
  }

  Salvar() {
    let permissaoFacility: PermissaoFacilityComponent = new PermissaoFacilityComponent();
    permissaoFacility.listaPermissao = this.listaPermissoes;
    permissaoFacility.facilityid = this.facility.Id;

    this.facilityservice.SalvaPermissao(permissaoFacility).subscribe(
      msg => {
        if (!msg.erro) {
          this.facilityservice.showToast(
            "success",
            "Permissões salva com sucesso!"
          );
          this.closeModal();
        } else {
          this.ShowErrors(msg.erro);
        }
      },
      erro => {
        if (erro.status === 400) {
          this.InputErros(erro.error);
        }
        if (erro.status === 401 || erro.status === 403) {
          this.usuarioservice.showToast("warning", "Aceso negado.");
        }
        this.usuarioservice.showToast(
          "error",
          "Não foi possível salvar a permissão!"
        );
      }
    );
  }

  InputErros(validationErrorDictionary) {
    for (var fieldName in validationErrorDictionary) {
      if (validationErrorDictionary.hasOwnProperty(fieldName)) {
        switch (fieldName) {
          default:
            this.facilityservice.showToast(
              "error",
              validationErrorDictionary[fieldName]
            );
            break;
        }
      }
    }
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      this.usuarioservice.showToast("error", erro);
    });
  }
}
