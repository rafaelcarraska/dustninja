import { PacienteService } from "./../../../@core/data/paciente.service";
import { PacienteComponent } from "./../paciente.component";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { EmpresaService } from "../../../@core/data/empresa.service";
import { ToasterConfig } from "angular2-toaster";
import "./paciente-form.loader";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { LoginService } from "../../../@core/data/login.service";

@Component({
  moduleId: module.id,
  selector: "paciente",
  templateUrl: "./paciente-form.component.html",
  styleUrls: [
    "../../../@theme/styles/form.scss",
    "./paciente-form.component.scss"
  ]
})
export class PacienteFormComponent implements OnInit {

  paciente: PacienteComponent = new PacienteComponent();
  listaPaciente: PacienteComponent[] = [];
  pacienteservice: PacienteService;
  empresaservice: EmpresaService;

  master: boolean;
  dataNascimento: string;

  errors: string[];
  erroNome: string;
  erroNamePrefix: string;
  erroMiddleName: string;
  erroGiveName: string;

  constructor(
    private route: ActivatedRoute,
    private readonly router: Router,
    pacienteservice: PacienteService,
    empresaservice: EmpresaService,
    private modalService: NgbModal,
    private loginservice: LoginService,
  ) {
    this.paciente.status = true;
    this.pacienteservice = pacienteservice;
    this.empresaservice = empresaservice;

    this.route.params.subscribe(params => this.LoadPaciente(params.id));
  }
  config: ToasterConfig;

  ngOnInit(): void {
    this.loginservice.DadosBasicos().subscribe(
      dadosBascios => {
        this.master = dadosBascios.master;
      });
  }

  LoadPaciente(Id: string) {
    if (Id != "0") {
      this.pacienteservice.ListaPaciente(Id).subscribe(
        paciente => {
          if (!paciente)
            this.pacienteservice.showToast(
              "warning",
              "Nenhum paciente localizado."
            );
          this.paciente = paciente;
          let data = new Date(this.paciente.dataNascimento);
          this.dataNascimento = data.getFullYear() + '-' + ('0' + (data.getMonth() + 1)).slice(-2) + '-' + ('0' + data.getDate()).slice(-2);
        },
        erro => this.pacienteservice.showToast("error", erro)
      );
    }
  }

  filtroPaciente(pacienteId: string){
    this.router.navigate([`/pages/worklist/paciente:${pacienteId}`]);
  }

  Cancelar() {
    this.router.navigate(["/pages/paciente/paciente-grid"]);
  }

  Salvar() {
    this.paciente.dataNascimento = new Date(this.dataNascimento);
    this.LimparErros();
    this.pacienteservice.salva(this.paciente).subscribe(
      msg => {
        if (!msg.erro) {
          if (!this.paciente.Id && msg.id) {
            this.paciente.Id = msg.id;
            this.listaPaciente.push(this.paciente);
          }
          this.pacienteservice.showToast(
            "success",
            "Paciente salvo com sucesso!"
          );
          this.router.navigate(["/pages/paciente/paciente-grid"]);
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
          this.pacienteservice.showToast("warning", "Aceso negado.");
        }
        this.pacienteservice.showToast(
          "error",
          "Não foi possível salvar o paciente!"
        );
      }
    );
  }

  LimparErros() {
    this.errors = [];
    this.erroNome = "";
    this.erroNamePrefix = "";
    this.erroMiddleName = "";
    this.erroGiveName = "";
  }

  InputErros(validationErrorDictionary) {
    for (var fieldName in validationErrorDictionary) {
      if (validationErrorDictionary.hasOwnProperty(fieldName)) {
        switch (fieldName) {
          case "nome":
          this.erroNome = validationErrorDictionary[fieldName];
            break;
          case "namePrefix":
          this.erroNamePrefix = validationErrorDictionary[fieldName];
            break;
          case "middleName":
          this.erroMiddleName = validationErrorDictionary[fieldName];
            break;
          case "giveName":
          this.erroGiveName = validationErrorDictionary[fieldName];
            break;
          default:
          this.pacienteservice.showToast("error", validationErrorDictionary[fieldName]);
            break;
        }
      }
    }
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      console.log(erro);
      this.pacienteservice.showToast(
        "error",
        erro
      );
    });
  }

}
