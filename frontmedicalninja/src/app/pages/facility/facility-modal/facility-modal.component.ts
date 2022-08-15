import { PerfilService } from "./../../../@core/data/perfil.service";
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { FacilityComponent } from "../facility.component";
import { ComboboxComponent } from "../../../@core/model/combobox/combobox.component";
import { EnderecoService } from "../../../@core/data/endereco.service";
import { FacilityService } from "../../../@core/data/facility.service";
import { ContatoComponent } from "../../../@core/model/contato/contatoComponent";
import { PermissaoComponent } from "../../../@core/model/permissao/permissao.component";
import { TemplateImpressaoService } from "../../../@core/data/templateImpressao.service";

@Component({
  moduleId: module.id,
  selector: "./facility-modal",
  templateUrl: "./facility-modal.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./facility-modal.component.scss"
  ]
})
export class FacilityModalComponent {
  @Input()
  listaComboPerfil: ComboboxComponent[];
  listaComboPerfilSelecionados: ComboboxComponent[];
  listaComboPerfilModal: ComboboxComponent[];
  perfilUsuario: ComboboxComponent = new ComboboxComponent();

  modalHeader: string = "";
  facility: FacilityComponent = new FacilityComponent();
  listaFacilitys: FacilityComponent[] = [];
  listaPermissoes: PermissaoComponent[] = [];
  listaTemplateImpressao: ComboboxComponent[];
  listaUsuario: ComboboxComponent[];

  enderecoservice: EnderecoService;
  facilityservice: FacilityService;
  perfilservice: PerfilService;
  templateImpressaoService: TemplateImpressaoService;
  contato: ContatoComponent = new ContatoComponent();

  errors: string[];
  erroDescricao: string;
  erroRazaoSocial: string;
  listaPermissoesModal: any;

  constructor(
    private activeModal: NgbActiveModal,
    enderecoservice: EnderecoService,
    facilityservice: FacilityService,
    perfilservice: PerfilService,
    templateImpressaoService: TemplateImpressaoService,
  ) {
    this.facilityservice = facilityservice;
    this.enderecoservice = enderecoservice;
    this.perfilservice = perfilservice;
    this.templateImpressaoService = templateImpressaoService;
  }

  ngOnInit(){
    this.CarregarTemplateImpressao();
  }

  AddPerfilClick($event) {
    const value: string = (<HTMLSelectElement>event.srcElement).value;
    this.listaComboPerfilSelecionados.push(
      this.listaComboPerfil.filter(x => x.Id == value)[0]
    );
    let indice = this.listaComboPerfil.indexOf(
      this.listaComboPerfil.filter(x => x.Id == value)[0]
    );
    this.listaComboPerfil.splice(indice, 1);
  }

  CarregarTemplateImpressao(){
    this.listaTemplateImpressao.forEach(x => {
        x.status = this.facility.listaTemplateImpressao.filter(f => f == x.Id).length > 0;
    });
  }

  RemovePerfilClick($event) {
    const value: string = (<HTMLSelectElement>event.srcElement).value;
    this.listaComboPerfil.push(
      this.listaComboPerfilSelecionados.filter(x => x.Id == value)[0]
    );
    let indice = this.listaComboPerfilSelecionados.indexOf(
      this.listaComboPerfilSelecionados.filter(x => x.Id == value)[0]
    );
    this.listaComboPerfilSelecionados.splice(indice, 1);
  }

  AddPerfil() {
    this.listaComboPerfil.forEach(item => {
      this.listaComboPerfilSelecionados.push(item);
    });

    this.listaComboPerfil = [];
  }

  DelPerfil() {
    this.listaComboPerfilSelecionados.forEach(item => {
      this.listaComboPerfil.push(item);
    });

    this.listaComboPerfilSelecionados = [];
  }

  DeletarContato(contato: ContatoComponent) {
    let novoContato = this.facility.listaContato.slice(0);
    let indice = novoContato.indexOf(contato);
    novoContato.splice(indice, 1);
    this.facility.listaContato = novoContato;
  }

  closeModal() {
    this.activeModal.close();
  }

  Cancelar(){
    this.closeModal();
  }

  //TODO: Componentizar a aba de endereço
  BuscaCEP() {
    this.enderecoservice.buscarCEP(this.facility.endereco.cep).subscribe(
      endereco => {
        if (endereco) {
          this.facility.endereco.endereco = endereco["logradouro"];
          this.facility.endereco.bairro = endereco["bairro"];
          this.facility.endereco.cidade = endereco["localidade"];
          this.facility.endereco.uf = endereco["uf"];
        } else {
          console.log(
            "CEP: " + this.facility.endereco.cep + " não localizado."
          );
        }
      },
      erro => console.log("Erro ao buscar Cep: " + erro) //TODO: cria log com erros
    );
  }

  AddContato() {
    this.facility.listaContato.push(this.contato);
    this.contato = new ContatoComponent();
    this.contato.contato = "";
    this.contato.telefone = "";
  }

  AddETemplateImpressao(){
    this.facility.listaTemplateImpressao = [];
    this.listaTemplateImpressao.filter(x => x.status == true).forEach(x => {
      if(x.status){
        this.facility.listaTemplateImpressao.push(x.Id);
      }
    });
  }

  Salvar() {
    this.LimparErros();
    this.AddETemplateImpressao();
    this.facilityservice.salva(this.facility).subscribe(
      msg => {
        if (!msg.erro) {
          if (!this.facility.Id && msg.id) {
            this.facility.Id = msg.id;
            this.listaFacilitys.push(this.facility);
          }
          this.facilityservice.showToast(
            "success",
            "Unidade salva com sucesso!"
          );
          this.closeModal();
        } else {
          this.ShowErrors(msg.erro);
        }
      },
      erro => {
        console.log('erro', erro);
        if (erro.status === 400) {
          this.InputErros(erro.error);
        }
        if (erro.status === 401 || erro.status === 403) {
          this.facilityservice.showToast("warning", "Aceso negado.");
        }
        this.facilityservice.showToast(
          "error",
          "Não foi possível salvar a unidade!"
        );
      }
    );
  }

  LimparErros() {
    this.errors = [];
    this.erroDescricao = "";
    this.erroRazaoSocial = "";
  }

  InputErros(validationErrorDictionary) {
    for (var fieldName in validationErrorDictionary) {
      if (validationErrorDictionary.hasOwnProperty(fieldName)) {
        switch (fieldName) {
          case "descricao":
            this.erroDescricao = validationErrorDictionary[fieldName];
            break;
          case "razaoSocial":
            this.erroRazaoSocial = validationErrorDictionary[fieldName];
          break;
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
      this.facilityservice.showToast("error", erro);
    });
  }
}
