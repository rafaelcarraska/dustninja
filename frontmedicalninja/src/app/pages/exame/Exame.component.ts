import { PagerService } from './../../@core/data/pager.service';
import { WorklistService } from './../../@core/data/worklist.service';
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ToasterConfig } from "angular2-toaster";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { LoginService } from "../../@core/data/login.service";
import { FileDCMComponent } from "../../@core/model/fileDCM/fileDCM.component";
import { ExameService } from "../../@core/data/exame.service";
import { PacienteService } from '../../@core/data/paciente.service';
import { PacienteComponent } from '../paciente/paciente.component';
import { ExamePacienteComponent } from './exame-paciente/exame-paciente.component';

@Component({
  selector: "exame",
  templateUrl: "./exame.component.html",
  styleUrls: [
    "./exame.component.scss"
  ]
})
export class ExameComponent implements OnInit {
  fileDCM: FileDCMComponent = new FileDCMComponent();

  exameservice: ExameService;
  worklistService: WorklistService;

  master: boolean;

  errors: string[];
  erroStudyDesc: string;
  erroBody_part: string;
  erroModality: string;

  constructor(
    private route: ActivatedRoute,
    private readonly router: Router,
    exameservice: ExameService,
    worklistService: WorklistService,

    private modalService: NgbModal,
    private loginservice: LoginService,
  ) {
    this.fileDCM.status = true;
    this.exameservice = exameservice;
    this.worklistService = worklistService;

    this.route.params.subscribe(params => this.LoadExame(params.id));
  }
  config: ToasterConfig;

  ngOnInit(): void {
    this.loginservice.DadosBasicos().subscribe(
      dadosBascios => {
        this.master = dadosBascios.master;
      });
  }

  LoadExame(Id: string) {
    if (Id != "0") {
      this.worklistService.listaExame(Id).subscribe(
        fileDCM => {
          if (!fileDCM)
            this.worklistService.showToast(
              "warning",
              "Exame não localizado."
            );
          this.fileDCM = fileDCM;
          // this.LoadGridPacientes();
        },
        erro => this.worklistService.showToast("error", erro)
      );
    }
  }

  AlterarPaciente(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(ExamePacienteComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout",
      windowClass : "modalxxl"
    });
    activeModal.componentInstance.fileDCM = fileDCM;
    activeModal.result.then(() => {
      this.LoadExame(this.fileDCM.Id);
    });
  }

  Salvar() {
    this.validacao();
    this.LimparErros();
    this.exameservice.salva(this.fileDCM).subscribe(
      msg => {
        if (!msg.erro) {
          this.exameservice.showToast(
            "success",
            "exame salvo com sucesso!"
          );
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
          this.exameservice.showToast("warning", "Aceso negado.");
        }
        this.exameservice.showToast(
          "error",
          "Não foi possível salvar o exame!"
        );
      }
    );
  }

  Cancelar(){
    this.router.navigate(["/pages/worklist"]);
  }

  validacao() {
    if (this.fileDCM.studyDesc == "") {
      this.erroStudyDesc = "O campo descrição é obrigatório.";
    }
    if (this.fileDCM.body_part == "") {
      this.erroBody_part = "O campo estudo é obrigatório.";
    }
    if (this.fileDCM.modality == "") {
      this.erroModality = "O campo modalidade é obrigatório.";
    }
  }

  LimparErros() {
    this.errors = [];
    this.erroStudyDesc = "";
    this.erroBody_part = "";
    this.erroModality = "";
  }

  InputErros(validationErrorDictionary) {
    for (var fieldName in validationErrorDictionary) {
      if (validationErrorDictionary.hasOwnProperty(fieldName)) {
        switch (fieldName) {
          case "studyDesc":
          this.erroStudyDesc = validationErrorDictionary[fieldName];
            break;
          case "body_part":
          this.erroBody_part = validationErrorDictionary[fieldName];
            break;
          case "modality":
          this.erroModality = validationErrorDictionary[fieldName];
            break;
          default:
          this.exameservice.showToast("error", validationErrorDictionary[fieldName]);
            break;
        }
      }
    }
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      console.log(erro); //toaster
      this.exameservice.showToast(
        "error",
        erro
      );
    });
  }

}
