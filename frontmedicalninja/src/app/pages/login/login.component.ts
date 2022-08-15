import { ComboboxComponent } from "./../../@core/model/combobox/combobox.component";
import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { LoginService } from "../../@core/data/login.service";
import { AutenticacaoComponent } from "../../@core/model/autenticacao/autenticacao.component";
import { EmpresaService } from "../../@core/data/empresa.service";


@Component({
  selector: "login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"]
})
export class LoginComponent {
  loginservice: LoginService;
  autenticacao: AutenticacaoComponent = new AutenticacaoComponent();
  returnLogin: string;
  empresaservice: EmpresaService;
  twofactor: Boolean;

  listaEmpresa: ComboboxComponent[];

  validacao: boolean;
  load: boolean;

  constructor(
    empresaservice: EmpresaService,
    loginservice: LoginService,
    private readonly router: Router,

  ) {
    this.loginservice = loginservice;
    this.empresaservice = empresaservice;
  }

  ngAfterViewInit(){
    sessionStorage.clear();
    this.validacao = false;
    this.load = false;
    this.Sair();
  }

  teste(){
    // alert('teste');
  }

  SelecionarEmpresa() {
    this.load = true;
    this.returnLogin = "Aguarde, estamos validando os dados.";
    this.loginservice.SelecionaEmpresa(this.autenticacao).subscribe(
      msg => {
        if (!msg.erro) {
          this.returnLogin = "";
          this.validacao = true;
          this.load = false;
          this.autenticacao.empresaId = msg.comboBox[0].Id;

          if (msg.comboBox.length == 1) {
            this.Logar();
          }
          this.listaEmpresa = msg.comboBox;
          this.twofactor = msg.twofactor;
        } else {
          this.returnLogin = msg.erro;
          this.load = false;
        }
      },
      erro => {
        this.returnLogin = "";
        this.load = false;
        if (erro.status === 400) {
          this.returnLogin = erro.text();
        }
        this.returnLogin = "Não foi possível realizar login";
      }
    );
  }

  onKeydown(){
    console.log('entrer');
  }

  Sair() {
    this.loginservice.Sair().subscribe(msg => {
      // console.log(msg);
    },erro => {
      console.log("Não foi possível realizar logoff");
    });
  }

  onChange(value: string) {
    this.autenticacao.empresaId = value;
  }

  Logar() {
    this.load = true;
    this.returnLogin = "Aguarde, estamos validando os dados.";
    this.loginservice.Logar(this.autenticacao).subscribe(
      msg => {
        if (!msg.erro) {
          this.returnLogin = "";
          this.load = false;
          if (msg.id) {
            sessionStorage.setItem("token", msg.id);
            this.router.navigate(["/pages/worklist"]);
          }
        } else {
          this.returnLogin = msg.erro[0];
          this.load = false;
        }
      },
      erro => {
        this.load = false;
        this.returnLogin = "";
        if (erro.status === 400) {
          this.returnLogin = erro.text();
        }
        this.returnLogin = "Não foi possível realizar login";
      }
    );
  }


}
