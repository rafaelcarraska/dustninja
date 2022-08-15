import { FacilityService } from "./../../../@core/data/facility.service";
import { ComboboxComponent } from "../../../@core/model/combobox/combobox.component";
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { UsuarioService } from "../../../@core/data/usuario.service";
import { UsuarioComponent } from "../usuario.component";
import { PermissaoComponent } from "../../../@core/model/permissao/permissao.component";
import { PermissaoUsuarioComponent } from "../../../@core/model/permissaoUsuario/permissaoUsuario.component";
import { batchPermissoesComponent } from "../../../@core/model/batchPermissoes/batchPermissoes.component";

@Component({
  moduleId: module.id,
  selector: "./usuario-permissao",
  templateUrl: "./usuario-permissao.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./usuario-permissao.component.scss"
  ]
})
export class UsuarioPermissaoComponent {
  @Input()
  usuarioservice: UsuarioService;
  facilityservice: FacilityService;

  usuario: UsuarioComponent = new UsuarioComponent();
  modalHeader: string = "";

  listaPermissoes: PermissaoComponent[] = [];

  listaFacilityCombo: ComboboxComponent[];
  batchPermissoesUsuario: batchPermissoesComponent = new batchPermissoesComponent();
  facilitySelecionada: string;
  listaPermissoesModal: any;
  listaPermissoesModalBatch: any;
  batch: boolean;
  listaFacility: ComboboxComponent[];

  constructor(
    private activeModal: NgbActiveModal,
    usuarioservice: UsuarioService,
    facilityservice: FacilityService
  ) {
    this.usuarioservice = usuarioservice;
    this.facilityservice = facilityservice;
    this.facilitySelecionada = "";

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
      this.batchPermissoesUsuario.usuarioId = this.usuario.Id;
      this.batchPermissoesUsuario.listUsuarios = this.listaFacility.filter(
        x => x.status == true
      );
      this.batchPermissoesUsuario.listaPermissoes = this.listaPermissoesModalBatch.filter(
        x => x.status == true
      );

      this.usuarioservice
      .ProcessarBatch(this.batchPermissoesUsuario)
      .subscribe(
        msg => {
          if (!msg.erro) {
            this.usuarioservice.showToast(
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
            this.usuarioservice.showToast("warning", "Aceso negado.");
          }
          this.usuarioservice.showToast(
            "error",
            "Não foi possível processar o batch!"
          );
        }
      );
    }
  }

  ngAfterViewInit() {
    this.LoadFacility();
  }

  checkAll(ev) {
    this.listaFacility.forEach(x => {
      x.status = ev.target.checked;
      this.checkFacility(x);
    });
  }

  LoadFacility() {
    this.facilityservice.ListaComboByUsuario(this.usuario.Id).subscribe(
      listaFacility => {
        if (!listaFacility) console.log("Nenhuma unidade localizada.");

        this.listaFacility = listaFacility;
        this.CarregarPermissaoUsuario();
      },
      erro => {
        this.facilityservice.erro(erro);
      }
    );
  }

  CarregarPermissaoUsuario() {
    if (this.usuario.Id != "") {
      this.usuarioservice.ListaPermissao(this.usuario.Id).subscribe(
        permissao => {
          this.listaPermissoes = permissao;
          this.PermissaoUsuario();
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.usuarioservice.showToast("warning", "Aceso negado.");
          } else {
            this.usuarioservice.showToast(
              "error",
              "Erro ao carregar permissoes."
            );
          }
        }
      );
    }
  }

  PermissaoUsuario() {
    this.listaFacility.forEach(x => {
      x.status =
        this.listaPermissoes.filter(p => p.facilityId == x.Id).length > 0;
    });
    this.listaFacilityCombo = this.listaFacility.filter(x => x.status == true);
  }

  CarregarPermissoes(facilitySelecionada: string) {
    let permissoes = this.listaPermissoes.filter(
      p => p.usuarioId == this.usuario.Id && p.facilityId == facilitySelecionada
    )[0].listaPermissao;

    this.listaPermissoesModal.forEach(x => {
      x.status = permissoes.filter(p => p == x.descricao).length > 0;
      // this.checkPermissao(x);
    });
  }

  checkPermissao(permissao) {
    let lista = this.listaPermissoes.filter(
      x =>
        x.facilityId == this.facilitySelecionada &&
        x.usuarioId == this.usuario.Id
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

  checkFacility(facility: ComboboxComponent) {
    let permisssao = new PermissaoComponent();
    if (facility.status) {
      permisssao.usuarioId = this.usuario.Id;
      permisssao.facilityId = facility.Id;
      permisssao.listaPermissao = [];
      this.listaPermissoes.push(permisssao);
    } else {
      let indice = this.listaPermissoes.indexOf(
        this.listaPermissoes.filter(x => x.facilityId == facility.Id)[0]
      );
      this.listaPermissoes.splice(indice, 1);
    }

    this.listaFacilityCombo = this.listaFacility.filter(x => x.status == true);
  }

  onChangeFacility(value: string) {
    if (value != "0") {
      this.listaPermissoesModal.forEach(element => {
        element.status = false;
      });
      this.facilitySelecionada = value;
      this.CarregarPermissoes(this.facilitySelecionada);
    }
  }

  closeModal() {
    this.activeModal.close();
  }

  Salvar() {
    let permissaoUsuario: PermissaoUsuarioComponent = new PermissaoUsuarioComponent();
    permissaoUsuario.listaPermissao = this.listaPermissoes;
    permissaoUsuario.usuarioId = this.usuario.Id;

    this.usuarioservice.SalvaPermissao(permissaoUsuario).subscribe(
      msg => {
        if (!msg.erro) {
          this.usuarioservice.showToast(
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
          "Não foi possível salvar a unidade!"
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
