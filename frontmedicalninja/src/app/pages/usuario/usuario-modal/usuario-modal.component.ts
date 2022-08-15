
import { environment } from "./../../../../environments/environment";
import { HttpClient, HttpEventType, HttpRequest } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { EnderecoService } from "../../../@core/data/endereco.service";
import { UsuarioComponent } from "../usuario.component";
import { UsuarioService } from "./../../../@core/data/usuario.service";
import { ContatoComponent } from "./../../../@core/model/contato/contatoComponent";
import { ComboboxComponent } from "../../../@core/model/combobox/combobox.component";
import { LoginService } from "../../../@core/data/login.service";

@Component({
  moduleId: module.id,
  selector: "./usuario-modal",
  templateUrl: "./usuario-modal.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./usuario-modal.component.scss"
  ]
})
export class UsuarioModalComponent {
  @Input()
  listaComboPerfil: ComboboxComponent[];
  perfilUsuario: ComboboxComponent = new ComboboxComponent();
  listaEmpresa: ComboboxComponent[];

  modalHeader: string = "";
  master: boolean;
  usuario: UsuarioComponent = new UsuarioComponent();
  listaUsuarios: UsuarioComponent[] = [];

  enderecoservice: EnderecoService;
  usuarioservice: UsuarioService;
  contato: ContatoComponent = new ContatoComponent();

  errors: string[];
  erroNome: string;
  senha: string;
  confirmeSenha: string;

  public progress: number;

  constructor(
    private activeModal: NgbActiveModal,
    enderecoservice: EnderecoService,
    usuarioservice: UsuarioService,
    private loginservice: LoginService,
    private http: HttpClient
  ) {
    this.usuarioservice = usuarioservice;
    this.enderecoservice = enderecoservice;
  }

  ngOnInit(){
    this.CarregarEmpresa();

    this.loginservice.DadosBasicos().subscribe(
      dadosBascios => {
        // console.log('dados básicos', dadosBascios);
        this.master = dadosBascios.master;
        environment.master = dadosBascios.master;
      });
  }

  upload(files) {
    if (files.length === 0) return;

    const formData = new FormData();

    for (let file of files) formData.append(this.usuario.Id, file);

    const req = new HttpRequest(
      "POST",
      environment.serviceUrl + `Upload/Assinatura`,
      formData,
      {
        reportProgress: true
      }
    );

    this.http.request(req).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress)
        this.progress = Math.round((100 * event.loaded) / event.total);
      else if (event.type === HttpEventType.Response) {
        this.usuario.assinatura.arquivo = event.body["id"];
        if (event.body["erro"]) {
          this.ShowErrors(event.body["erro"]);
        } else {
          this.usuarioservice.showToast(
            "success",
            "Upload realizado com sucesso!"
          );
        }
      }
    });
  }

  DeletarContato(contato: ContatoComponent) {
    let novoContato = this.usuario.listaContato.slice(0);
    let indice = novoContato.indexOf(contato);
    novoContato.splice(indice, 1);
    this.usuario.listaContato = novoContato;
  }

  onChange(value: string) {
    this.usuario.perfilId = value;
  }

  closeModal() {
    this.activeModal.close();
  }

  //TODO: Componentizar a aba de endereço
  BuscaCEP() {
    this.enderecoservice.buscarCEP(this.usuario.endereco.cep).subscribe(
      endereco => {
        if (endereco) {
          this.usuario.endereco.endereco = endereco["logradouro"];
          this.usuario.endereco.bairro = endereco["bairro"];
          this.usuario.endereco.cidade = endereco["localidade"];
          this.usuario.endereco.uf = endereco["uf"];
        } else {
          console.log("CEP: " + this.usuario.endereco.cep + " não localizado.");
        }
      },
      erro => console.log("Erro ao buscar Cep: " + erro) //TODO: cria log com erros
    );
  }

  CarregarEmpresa(){
    this.listaEmpresa.forEach(x => {
        x.status = this.usuario.listaEmpresa.filter(f => f == x.Id).length > 0;
    });
  }

  AddContato() {
    this.usuario.listaContato.push(this.contato);
    this.contato = new ContatoComponent();
    this.contato.contato = '';
    this.contato.telefone = '';
  }

  AddEmpresa(){
    this.usuario.listaEmpresa = [];
    this.listaEmpresa.filter(x => x.status == true).forEach(x => {
      if(x.status){
        this.usuario.listaEmpresa.push(x.Id);
      }
    });
  }

  Salvar() {
    this.LimparErros();
    if (this.senha == this.confirmeSenha) {
      this.AddEmpresa();
      this.usuarioservice.salva(this.usuario, this.senha).subscribe(
        msg => {
          if (!msg.erro) {
            if (!this.usuario.Id && msg.id) {
              this.usuario.Id = msg.id;
              this.listaUsuarios.push(this.usuario);
            }
            this.usuarioservice.showToast(
              "success",
              "Usuário salvo com sucesso!"
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
            "Não foi possível salvar o Usuário!"
          );
        }
      );
    } else {
      this.usuarioservice.showToast(
        "error",
        "A Senha e confirmação não confere."
      );
    }
  }

  LimparErros() {
    this.errors = [];
    this.erroNome = "";
  }

  InputErros(validationErrorDictionary) {
    for (var fieldName in validationErrorDictionary) {
      if (validationErrorDictionary.hasOwnProperty(fieldName)) {
        switch (fieldName) {
          case "nome":
          this.erroNome = validationErrorDictionary[fieldName];
            break;
          default:
          this.usuarioservice.showToast("error", validationErrorDictionary[fieldName]);
            break;
        }
      }
    }
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      console.log(erro);
      this.usuarioservice.showToast("error", erro);
    });
  }
}
