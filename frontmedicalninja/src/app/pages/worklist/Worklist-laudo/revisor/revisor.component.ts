import { Component, Input, EventEmitter, Output } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FileDCMComponent } from "../../../../@core/model/fileDCM/fileDCM.component";
import { ExameInfoComponent } from "../../../../@core/model/exameInfo/exameInfo.component";
import { ExameService } from "../../../../@core/data/exame.service";

@Component({
  moduleId: module.id,
  selector: "./revisor",
  templateUrl: "./revisor.component.html",
  styleUrls: [
    "../../../../@theme/styles/modal.scss",
    "./revisor.component.scss"
  ]
})
export class RevisorComponent {
  @Output() passEntry: EventEmitter<any> = new EventEmitter();
  @Input()
  modalHeader: string = "";
  fileDCM: FileDCMComponent = new FileDCMComponent();
  exameInfo: ExameInfoComponent = new ExameInfoComponent();
  exameService: ExameService;

  constructor(
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    exameService: ExameService,
  ) {
    this.exameService = exameService;
  }

  ngOnInit(){
    this.exameService.ListaExameInfoLaudado(this.fileDCM.Id).subscribe(
        exameInfo => {
          this.exameInfo = exameInfo;
          this.modalHeader = `Esse exame foi laudado por ${exameInfo.usuario} (${exameInfo.login}) em ${exameInfo.data}. Deseja substituir a primeira assinatura ou assinar como revisor (segunda assinatura)?`;
        },
        erro => {
          this.exameService.erro(erro);
        }
      );
  }

  closeModal(){
    this.activeModal.close();
  }

  Primeira(){
    this.passEntry.emit(true);
    this.closeModal();
  }

  Segunda(){
    this.passEntry.emit(false);
    this.closeModal();
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.exameService.showToast("warning", erro);
      });
    }
  }
}
