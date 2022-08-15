import { TemplateImpressaoService } from './../../../@core/data/templateImpressao.service';
import { ExameService } from './../../../@core/data/exame.service';
import { FileDCMComponent } from './../../../@core/model/fileDCM/fileDCM.component';
import { environment } from "./../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbActiveModal, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ConfirmacaoComponent } from '../../../@core/model/fileDCM/confirmacao.component';
import { PrioridadeComponent } from '../../../@core/model/Prioridade/prioridadeComponent';
import { FacilityService } from '../../../@core/data/facility.service';
import { ComboboxComponent } from '../../../@core/model/combobox/combobox.component';
import { WorklistNotasComponent } from '../worklist-notas/worklist-notas.component';
import { WorklistAnexosComponent } from '../worklist-anexos/worklist-anexos.component';
import { WorklistDesmembrarComponent } from '../worklist-desmembrar/worklist-desmembrar.component';

@Component({
  moduleId: module.id,
  selector: "./worklist-confirmacao",
  templateUrl: "./worklist-confirmacao.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./worklist-confirmacao.component.scss"
  ]
})
export class WorklistConfirmacaoComponent {
  @Input()
  modalHeader: string = "";
  fileDCM: FileDCMComponent = new FileDCMComponent();
  confirmacao: ConfirmacaoComponent = new ConfirmacaoComponent();
  prioridade: PrioridadeComponent = new PrioridadeComponent();
  recent: any[];
  listaTemplateImpressao: ComboboxComponent[];
  serviceUrlDCM: string = environment.serviceUrlDCM;
  serviceUrlOHIF: string = environment.serviceUrlOHIF;

  usuarioId: string = environment.usuarioId;
  exameService: ExameService;
  facilityService: FacilityService;
  templateImpressaoService: TemplateImpressaoService;

  constructor(
    private activeModal: NgbActiveModal,
    exameService: ExameService,
    private modalService: NgbModal,
    facilityService: FacilityService,
    templateImpressaoService: TemplateImpressaoService,
    private http: HttpClient
  ) {
    this.exameService = exameService;
    this.facilityService = facilityService;
    this.templateImpressaoService = templateImpressaoService;
  }

  ngOnInit() {
    this.LoadTemplateImpressao();
    this.VerificaPrioridadeFacility();
  }

  closeModal() {
    this.activeModal.close();
  }

  Notas(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(WorklistNotasComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.modalHeader = fileDCM.studyDesc;
    activeModal.componentInstance.fileDCM = fileDCM;
  }

  Anexos(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(WorklistAnexosComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.modalHeader = fileDCM.studyDesc;
    activeModal.componentInstance.fileDCM = fileDCM;
  }

  AbrirModalDesmembrar(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(WorklistDesmembrarComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.fileDCM = fileDCM;
    activeModal.componentInstance.confirmacao = true;
    activeModal.componentInstance.passEntry.subscribe((receivedEntry) => {
      if (receivedEntry) {
        this.confirmacao.desmembrar = receivedEntry;
        // console.log(receivedEntry);
      }
    });
    activeModal.result.then(() => {
      console.log('modal desmebrar fechado');
    });
  }

  Confirmacao(statusExames: string){
    this.confirmacao.fileDCMId = this.fileDCM.Id;
    this.confirmacao.statusExames = statusExames;
    if(this.confirmacao.prioridade == null){
      this.confirmacao.prioridade = 'normal';
    }
    switch (statusExames) {
      case "laudar":
        if(this.confirmacao.templateImpressaoid){
          this.SalvarConfirmacao();
        }else{
          this.exameService.showToast(
            "error",
            "Nenhum template de impressão selecionado!"
          );
        }
        break;
      case "comparacao":
          this.SalvarConfirmacao();
        break;
      case "desconsiderado":
          this.SalvarConfirmacao();
        break;
      default:
        this.exameService.showToast(
          "error",
          "Açao não localizada!"
        );
        break;
    }
  }

  LoadTemplateImpressao() {
    this.templateImpressaoService.ListaComboByFacility(this.fileDCM.facilityId).subscribe(
      listaTemplateImpressao => {
        if (!listaTemplateImpressao) {
          console.log("Nenhum template de impressão localizado.");
        }
        else{
          this.listaTemplateImpressao = listaTemplateImpressao;
          this.confirmacao.templateImpressaoid = listaTemplateImpressao[0].Id;
        }
      },
      erro => {
        this.templateImpressaoService.erro(erro);
      }
    );
  }

  SalvarConfirmacao(){
    this.exameService.Confirmacao(this.confirmacao).subscribe(
      msg => {
        if (!msg.erro) {
          this.exameService.showToast(
            "success",
            "Confirmação de Exame realizada com sucesso!"
          );
          this.fileDCM.statusExamesFormatado = msg.id; //id retorna o novo status do exame
          this.fileDCM.prioridade = this.confirmacao.prioridade;
          this.closeModal();
        } else {
          this.ShowErrors(msg.erro);
        }
      },
      erro => {
        this.exameService.showToast(
          "error",
          "Não foi possível realizar a confirmação!"
        );
      }
    );

  }

  VerificaPrioridadeFacility(){
    this.facilityService.PrioridadeFacility(this.fileDCM.facilityId).subscribe(
      prioridade => {
        if (!prioridade){
        this.facilityService.showToast(
            "error",
            "Não foi possível verificar o status do exame!"
          );
        }else{
          this.prioridade = prioridade;
          this.confirmacao.prioridade = 'normal';
        }
      },
      erro => {
        this.facilityService.erro(erro);
      }
    );
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.exameService.showToast("warning", erro);
      });
    }
  }
}
