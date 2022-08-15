import { FileDCMComponent } from '../../../@core/model/fileDCM/fileDCM.component';
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { WorklistConfirmacaoComponent } from '../worklist-confirmacao/worklist-confirmacao.component';
import { ExameService } from '../../../@core/data/exame.service';
import { ConfirmacaoComponent } from '../../../@core/model/fileDCM/confirmacao.component';

@Component({
  moduleId: module.id,
  selector: "./worklist-status",
  templateUrl: "./worklist-status.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./worklist-status.component.scss"
  ]
})
export class WorklistStatusComponent {
  @Input()
  modalHeader: string = "";
  fileDCM: FileDCMComponent = new FileDCMComponent();
  confirmacao: ConfirmacaoComponent = new ConfirmacaoComponent();
  exameService: ExameService;

  constructor(
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    exameService: ExameService,
  ) {
    this.exameService = exameService;
  }

  ngOnInit(){
  }

  closeModal(){
    this.activeModal.close();
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        // this.exameService.showToast("warning", erro);
      });
    }
  }

  AbrirConfirmacao(){
    this.closeModal();
    const activeModal = this.modalService.open(WorklistConfirmacaoComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.modalHeader = this.fileDCM.studyDesc;
    activeModal.componentInstance.fileDCM = this.fileDCM;
  }

  Laudar(){
    this.closeModal();
    if(this.fileDCM.permitirLaudo){
      this.confirmacao.fileDCMId = this.fileDCM.Id;
      this.confirmacao.prioridade = this.fileDCM.prioridade;
      this.confirmacao.statusExames = "laudando";
      this.confirmacao.subStatusExames = this.fileDCM.subStatusExamesFormatado;

      this.exameService.Confirmacao(this.confirmacao).subscribe(
        msg => {
          if (!msg.erro) {
            this.fileDCM.statusExamesFormatado = "laudando";
            window.open(`./#/pages/laudo/${this.fileDCM.Id}`, "laudo", 'width=1200,height=900');
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          this.exameService.showToast(
            "error",
            "Não foi possível Salvar o laudo!"
          );
        }
      );
    }else{
      this.exameService.showToast(
        "warning",
        "Você não possui permissão para lauda um exame!"
      );
    }
  }
}
