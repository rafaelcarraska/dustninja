import { MascaraLaudoService } from "./../../../@core/data/mascaraLaudo.service";
import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { TipoExameService } from "./../../../@core/data/tipoExame.service";
import { TipoExameComponent } from "./../tipoExame.component";
import { ComboboxComponent } from "../../../@core/model/combobox/combobox.component";

@Component({
  moduleId: module.id,
  selector: "tipoExame-modal",
  templateUrl: "./tipoExame-modal.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./tipoExame-modal.component.scss"
  ]
})
export class TipoExameModalComponent {
  listaComboMascara: ComboboxComponent[];
  nodes = [];

  modalHeader: string = "";
  tipoExame: TipoExameComponent = new TipoExameComponent();
  listaTipoExames: TipoExameComponent[] = [];
  tipoExameservice: TipoExameService;
  mascaraservice: MascaraLaudoService;

  errors: string[];
  erroNome: string;
  erroModalidade: string;
  erroCodCobranca: string;
  erroPalavraChave: string;
  erroMascara: string;

  constructor(
    private activeModal: NgbActiveModal,
    tipoExameservice: TipoExameService,
    mascaraservice: MascaraLaudoService
  ) {
    this.tipoExameservice = tipoExameservice;
    this.mascaraservice = mascaraservice;
  }

  ngAfterViewInit() {
    this.LoadMascara();
  }

  LoadMascara() {
    this.mascaraservice.listaCombo().subscribe(
      listaMascara => {
        if (!listaMascara) console.log("Nenhuma máscara de laudo localizada.");

        this.listaComboMascara = listaMascara;
      },
      erro => {
        this.mascaraservice.erro(erro);
      }
    );
  }

  closeModal() {
    this.activeModal.close();
  }

  Salvar() {
    this.validacao();
    this.LimparErros();
    this.tipoExameservice.salva(this.tipoExame).subscribe(
      msg => {
        if (!msg.erro) {
          if (!this.tipoExame.Id && msg.id) {
            this.tipoExame.Id = msg.id;
            this.listaTipoExames.push(this.tipoExame);
          }
          this.tipoExameservice.showToast(
            "success",
            "TipoExame salvo com sucesso!"
          );
          this.closeModal();
        } else {
          this.ShowErrors(msg.erro);
        }
      },
      erro => {
        console.log(erro);
        if (erro.status === 400) {
          this.InputErros(erro.error);
        }
        if (erro.status === 401 || erro.status === 403) {
          this.tipoExameservice.showToast("warning", "Aceso negado.");
        }
        this.tipoExameservice.showToast(
          "error",
          "Não foi possível salvar o tipoExame!"
        );
      }
    );
  }

  onChange(value: string) {
    this.tipoExame.mascaraLaudoId = value;
  }

  validacao() {
    if (this.tipoExame.mascaraLaudoId == "") {
      this.erroMascara = "Obrigatório selecionar uma máscara de laudo.";
    }
  }

  LimparErros() {
    this.errors = [];
    this.erroNome = "";
    this.erroModalidade = "";
    this.erroCodCobranca = "";
    this.erroPalavraChave = "";
    this.erroMascara = "";
  }

  InputErros(validationErrorDictionary) {
    for (var fieldName in validationErrorDictionary) {
      if (validationErrorDictionary.hasOwnProperty(fieldName)) {
        switch (fieldName) {
          case "nome":
            this.erroNome = validationErrorDictionary[fieldName];
            break;
          case "modalidade":
            this.erroModalidade = validationErrorDictionary[fieldName];
            break;
          case "codCobranca":
            this.erroCodCobranca = validationErrorDictionary[fieldName];
            break;
          case "palavraChave":
            this.erroPalavraChave = validationErrorDictionary[fieldName];
            break;
          case "mascaraLaudoId":
            this.erroMascara = validationErrorDictionary[fieldName];
            break;
          default:
            this.tipoExameservice.showToast(
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
      console.log(erro);
      this.tipoExameservice.showToast("error", erro);
    });
  }
}
