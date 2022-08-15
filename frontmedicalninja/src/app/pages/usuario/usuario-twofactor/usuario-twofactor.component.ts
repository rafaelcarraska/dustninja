import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { UsuarioService } from "../../../@core/data/usuario.service";
import { UsuarioComponent } from "../usuario.component";
import { TwoFactorComponent } from "../../../@core/model/autenticacao/twoFactor.component";
import { ValidaTwoFactorComponent } from "../../../@core/model/autenticacao/validaTwoFactor.component";

@Component({
  moduleId: module.id,
  selector: "./usuario-twofactor",
  templateUrl: "./usuario-twofactor.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./usuario-twofactor.component.scss"
  ]
})
export class UsuarioTwoFactorComponent {
  @Input()
  usuarioservice: UsuarioService;
  twoFactor: TwoFactorComponent = new TwoFactorComponent();
  validaTwoFactor: ValidaTwoFactorComponent = new ValidaTwoFactorComponent();


  usuario: UsuarioComponent = new UsuarioComponent();
  modalHeader: string = "";

  constructor(
    private activeModal: NgbActiveModal,
    usuarioservice: UsuarioService,
  ) {
    this.usuarioservice = usuarioservice;


  }

  ngAfterViewInit() {
    this.LoadTwoFactor();
  }

  LoadTwoFactor() {
    this.usuarioservice.ListaTwoFactor(this.usuario.Id).subscribe(
      twoFactor => {
        if (!twoFactor) {
          console.log("Não foi possivel carregar as informações.");
        }
        this.twoFactor = twoFactor;
        console.log(twoFactor);
      },
      erro => {
        this.usuarioservice.erro(erro);
      }
    );
  }

  closeModal() {
    this.activeModal.close();
  }

  ValidaTwoFactor(){
    this.validaTwoFactor.usuarioId = this.usuario.Id;
    this.usuarioservice.ValidaTwoFactor(this.validaTwoFactor).subscribe(
      result => {
        if (!result) {
          this.usuarioservice.showToast(
            "success",
            "Chave valida!"
          );
        } else {
          this.usuarioservice.showToast(
            "error",
            "Chave invalida!"
          );
        }
      },
      erro => {
        if (erro.status === 400) {
          this.usuarioservice.showToast("error", erro.erro);
        }
        if (erro.status === 401 || erro.status === 403) {
          this.usuarioservice.showToast("warning", "Aceso negado.");
        }
        this.usuarioservice.showToast(
          "error",
          "Não foi possível validar a chave!"
        );
      }
    );
  }

  Gerar(usuarioid: string) {
    this.usuarioservice.NovoTwoFactor(usuarioid).subscribe(
      msg => {
        if (!msg.erro) {
          this.usuarioservice.showToast(
            "success",
            "nova chave gerada com sucesso!"
          );
          this.LoadTwoFactor();
        } else {
          this.ShowErrors(msg.erro);
        }
      },
      erro => {
        if (erro.status === 400) {
          this.usuarioservice.showToast("error", erro.erro);
        }
        if (erro.status === 401 || erro.status === 403) {
          this.usuarioservice.showToast("warning", "Aceso negado.");
        }
        this.usuarioservice.showToast(
          "error",
          "Não foi possível gerar nova chave!"
        );
      }
    );
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      this.usuarioservice.showToast("error", erro);
    });
  }
}
