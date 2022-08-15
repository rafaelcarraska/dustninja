import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { EmpresaComponent } from "../empresa.component";
import { EnderecoService } from "../../../@core/data/endereco.service";
import { EmpresaService } from "../../../@core/data/empresa.service";
import { ContatoComponent } from "../../../@core/model/contato/contatoComponent";

@Component({
  moduleId: module.id,
  selector: "./empresa-modal",
  templateUrl: "./empresa-modal.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./empresa-modal.component.scss"
  ]
})
export class EmpresaModalComponent {
  modalHeader: string = "";

  empresa: EmpresaComponent = new EmpresaComponent();
  listaEmpresa: EmpresaComponent[] = [];
  // @Output() acao = new EventEmitter();
  enderecoservice: EnderecoService;
  empresaservice: EmpresaService;

  errors: string[];
  erroRazaoSocial: string;
  erroNomeFantasia: string;

  contato: ContatoComponent = new ContatoComponent();

  constructor(
    private activeModal: NgbActiveModal,
    empresaservice: EmpresaService,
    enderecoservice: EnderecoService,
    // contato: ContatoComponent
  ) {
    this.empresaservice = empresaservice;
    this.enderecoservice = enderecoservice;
    // this.contato = new ContatoComponent();
  }

  DeletarContato(contato: ContatoComponent) {
    let novoContato = this.empresa.listaContato.slice(0);
    let indice = novoContato.indexOf(contato);
    novoContato.splice(indice, 1);
    this.empresa.listaContato = novoContato;
  }

  closeModal() {
    this.activeModal.close();
  }

  //TODO: Componentizar a aba de endereço
  BuscaCEP() {
    this.enderecoservice.buscarCEP(this.empresa.endereco.cep).subscribe(
      endereco => {
        if (endereco) {
          this.empresa.endereco.endereco = endereco["logradouro"];
          this.empresa.endereco.bairro = endereco["bairro"];
          this.empresa.endereco.cidade = endereco["localidade"];
          this.empresa.endereco.uf = endereco["uf"];
        } else {
          console.log("CEP: " + this.empresa.endereco.cep + " não localizado.");
        }
      },
      erro => console.log("Erro ao buscar Cep: " + erro) //TODO: cria log com erros
    );
  }

  // executaAcao() {
  //   this.acao.emit(null);
  // }

  AddContato() {
    this.empresa.listaContato.push(this.contato);
    this.contato = new ContatoComponent();
    this.contato.contato = '';
    this.contato.telefone = '';
  }

  Salvar() {
    // event.preventDefault();
    this.LimparErros();

    this.empresaservice.salva(this.empresa).subscribe(
      msg => {
        if (!msg.erro) {
          if (msg.id) this.empresa.Id = msg.id;

          this.listaEmpresa.push(this.empresa);
          this.empresaservice.showToast(
            "success",
            "Empresa salva com sucesso!"
          );
          this.closeModal();
        } else {
          this.ShowErrors(msg.erro);
        }
      },
      erro => {
        console.log(erro); //remover
        if (erro.status === 400) {
          this.InputErros(erro.error);
        }
        if (erro.status === 401 || erro.status === 403) {
          this.empresaservice.showToast("warning", "Aceso negado.");
        }
        this.empresaservice.showToast(
          "error",
          "Não foi possível salvar a empresa!"
        );
      }
    );
  }

  LimparErros() {
    this.errors = [];
    this.erroNomeFantasia = "";
    this.erroRazaoSocial = "";
  }

  InputErros(validationErrorDictionary) {
    for (var fieldName in validationErrorDictionary) {
      if (validationErrorDictionary.hasOwnProperty(fieldName)) {
        switch (fieldName) {
          case "razaoSocial":
          this.erroRazaoSocial = validationErrorDictionary[fieldName];
            break;
            case "nomeFantasia":
          this.erroNomeFantasia = validationErrorDictionary[fieldName];
            break;
          default:
          this.empresaservice.showToast("error", validationErrorDictionary[fieldName]);
            break;
        }
      }
    }
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      console.log(erro); //toaster
      this.empresaservice.showToast(
        "error",
        erro
      );
    });
  }
}
