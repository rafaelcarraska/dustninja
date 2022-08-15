import { FileDCMComponent } from './../../../@core/model/fileDCM/fileDCM.component';
import { FileDCMNotaComponent } from "./../../../@core/model/NotaHistorico/fileDCMNota.component";
import { NotaComponent } from "./../../../@core/model/NotaHistorico/nota.component";
import { environment } from "./../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { LoginService } from "../../../@core/data/login.service";
import { WorklistService } from "../../../@core/data/worklist.service";
import { NotaHistoricoComponent } from '../../../@core/model/NotaHistorico/notaHistorico.component';

@Component({
  moduleId: module.id,
  selector: "./worklist-notas",
  templateUrl: "./worklist-notas.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./worklist-notas.component.scss"
  ]
})
export class WorklistNotasComponent {
  @Input()
  modalHeader: string = "";
  fileDCM: FileDCMComponent = new FileDCMComponent();
  recent: any[];
  nota: NotaComponent;
  fileDCMNota: FileDCMNotaComponent = new FileDCMNotaComponent();

  usuarioId: string = environment.usuarioId;


  worklistservice: WorklistService;

  constructor(
    private activeModal: NgbActiveModal,
    worklistservice: WorklistService,
    private loginservice: LoginService,
    private http: HttpClient
  ) {
    this.worklistservice = worklistservice;
  }

  ngOnInit() {
    this.modalHeader = `Notas - ${this.fileDCM.pacienteNome} - ${this.fileDCM.studyDesc} - ${this.fileDCM.body_part} - ${this.fileDCM.date_study_formatada}`
    this.LoadNota();
  }

  LoadNota() {
    this.fileDCMNota.fileDCMId = this.fileDCM.Id;
    this.worklistservice.listaNotas(this.fileDCM.Id).subscribe(
      nota => {
        if (nota) {
          this.nota = nota;
          this.fileDCMNota.Id = this.nota.Id;

        }else{
          this.nota = new NotaComponent();
        }
      },
      erro => {
        console.log(erro);
        this.worklistservice.erro(erro);
      }
    );
  }

  RemoveNota(notaHIst: NotaHistoricoComponent){
    let novoNota = this.nota.listaNota.slice(0);
    let indice = novoNota.indexOf(notaHIst);
    novoNota.splice(indice, 1);
    this.nota.listaNota = novoNota;
    this.nota.countNota--;
    this.worklistservice.DeletarNota(this.nota).subscribe(
      msg => {
        if (!msg.erro) {
          this.worklistservice.showToast(
            "success",
            "Nota removida com sucesso!"
          );
        } else {
          this.ShowErrors(msg.erro);
          this.LoadNota();
        }
      },
      erro => {
        this.worklistservice.showToast("warning", erro);
        if (erro.status === 401 || erro.status === 403) {
          this.worklistservice.showToast("warning", "Aceso negado.");
        }
        this.worklistservice.showToast(
          "error",
          "Não foi possível remover a nota!"
        );
        this.LoadNota();
      }
    );
  }

  closeModal() {
    this.activeModal.close();
  }

  Salvar() {
    // this.LimparErros();
    if (this.fileDCMNota.nota.trim() != "") {
      this.worklistservice.salvaNota(this.fileDCMNota).subscribe(
        msg => {
          if (!msg.erro) {
            this.fileDCM.pendente = true;
            this.fileDCM.favorito = true;
            this.worklistservice.showToast(
              "success",
              "Nota salva com sucesso!"
            );
            this.fileDCMNota.nota = "";
            this.LoadNota();
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.worklistservice.showToast("warning", "Aceso negado.");
          }
          this.worklistservice.showToast(
            "error",
            "Não foi possível salvar a Nota!"
          );
        }
      );
    } else {
      this.worklistservice.showToast(
        "error",
        "Nota Obrigatória."
      );
    }
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.worklistservice.showToast("warning", erro);
      });
    }
  }
}
