import { EventoComponent } from './../../../@core/model/evento/evento.component';
import { FileDCMComponent } from './../../../@core/model/fileDCM/fileDCM.component';
import { environment } from "./../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { LoginService } from "../../../@core/data/login.service";
import { WorklistService } from "../../../@core/data/worklist.service";

@Component({
  moduleId: module.id,
  selector: "./worklist-log",
  templateUrl: "./worklist-log.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./worklist-log.component.scss"
  ]
})
export class WorklistLogComponent {
  @Input()
  modalHeader: string = "";
  fileDCM: FileDCMComponent = new FileDCMComponent();
  listaEventos: EventoComponent[];
  recent: any[];

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
    this.LoadLog();
  }

  LoadLog() {
    this.worklistservice.listaeventos(this.fileDCM.Id).subscribe(
      listaEventos => {
        if (listaEventos) {
          this.listaEventos = listaEventos;
        }else{
          this.listaEventos = [];
        }
      },
      erro => {
        console.log(erro);
        this.worklistservice.erro(erro);
      }
    );
  }

  closeModal() {
    this.activeModal.close();
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.worklistservice.showToast("warning", erro);
      });
    }
  }
}
