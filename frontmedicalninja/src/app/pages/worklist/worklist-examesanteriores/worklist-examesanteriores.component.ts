import { FileDCMComponent } from '../../../@core/model/fileDCM/fileDCM.component';
import { HttpClient } from "@angular/common/http";
import { Component, Input, Output, EventEmitter } from "@angular/core";
import { NgbActiveModal, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { PagerService } from '../../../@core/data/pager.service';
import { ToasterConfig } from 'angular2-toaster';
import { WorklistService } from '../../../@core/data/worklist.service';
import { WorklistAnexosComponent } from '../worklist-anexos/worklist-anexos.component';
import { WorklistNotasComponent } from '../worklist-notas/worklist-notas.component';
import { TemplateImpressaoService } from '../../../@core/data/templateImpressao.service';
import { WorklistTemplateImpressaoComponent } from '../worklist-templateImpressao/worklist-templateImpressao.component';
import { environment } from '../../../../environments/environment';
import { ExameService } from '../../../@core/data/exame.service';

@Component({
  moduleId: module.id,
  selector: "./worklist-examesanteriores",
  templateUrl: "./worklist-examesanteriores.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "../../../@theme/styles/grid.scss",
    "./worklist-examesanteriores.component.scss"
  ]
})
export class WorklistExamesAnterioresComponent {
  @Output() passEntry: EventEmitter<any> = new EventEmitter();
  @Input()
  modalHeader: string = "";
  listaFileDCM: FileDCMComponent[] = [];
  load: boolean = false;
  copylaudo: boolean = false;
  public searchString: string;
  fileDCM: FileDCMComponent = new FileDCMComponent();
  pagerservice: PagerService;
  worklistservice: WorklistService;
  exameService: ExameService;
  templateImpressaoService: TemplateImpressaoService;
  serviceUrlOHIF: string = environment.serviceUrlOHIF;

  constructor(
    private activeModal: NgbActiveModal,
    private http: HttpClient,
    pagerservice: PagerService,
    worklistservice: WorklistService,
    private modalService: NgbModal,
    templateImpressaoService: TemplateImpressaoService,
    exameService: ExameService,
  ) {
    this.pagerservice = pagerservice;
    this.worklistservice = worklistservice;
    this.templateImpressaoService = templateImpressaoService;
    this.exameService = exameService;
  }
  config: ToasterConfig;
  pager: any = {};
  pagedItems: any[];

  ngAfterViewInit() {
    this.LoadGridExamesAnteriores();
  }

  closeModal() {
    this.activeModal.close();
  }

  LoadGridExamesAnteriores() {
    this.load = true;
    this.worklistservice.lista(`paciente:${this.fileDCM.paciente.Id}`).subscribe(
      listaFileDCM => {
        this.listaFileDCM = listaFileDCM;
        this.setfiltro();
        // console.log(listaFileDCM);
      },
      erro => {
        this.worklistservice.erro(erro);
      },
      () => {
        this.load = false;
      }
    );
  }

  dbfiltro(texto: string) {
    this.searchString = texto;
    this.setfiltro();
  }

  OpenOhif(fileDCM: FileDCMComponent) {
    let logView = this.exameService.montaLogVisualizacao('ohif', fileDCM.Id);
    this.exameService.LogVisualizacao(logView).subscribe();

    // let iframe = `<html><head>
    // <meta charset="utf-8"><title>Fastpacs</title>
    // <style>body, html {width: 100%; height: 100%; margin: 0; padding: 0}</style>
    // </head><body>
    // <iframe src="${this.serviceUrlOHIF}viewer/${fileDCM.studyId}" style="height:calc(100% - 4px);width:calc(100% - 4px)"></iframe>
    // </html></body>`;

    // let win = window.open('', "Ohif", 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,resizable=no');
    // // win.document.clear();
    // win.document.body.innerHTML = iframe;
    // // win.document.write(iframe);
    window.open(`${this.serviceUrlOHIF}viewer/${fileDCM.studyId}`, "Ohif", 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,resizable=no');
  }

  setfiltro() {
    if (!this.searchString || !Array.isArray(this.listaFileDCM)) {
      this.setPage(1);
      return;
    }

    let filter: any = {
      date_study_formatada: this.searchString,
      body_part: this.searchString,
      studyDesc: this.searchString,
      institution: this.searchString,
      pacienteNome: this.searchString,
      facilityDesc: this.searchString,
      statusExamesFormatado: this.searchString,
      prioridade: this.searchString
    };

    let filterKeys = Object.keys(filter);

    this.pagedItems = this.listaFileDCM.filter(item => {
      return filterKeys.some(keyName => {
        return (
          new RegExp(filter[keyName], "gi").test(item[keyName]) || filter[keyName] == "");
      });
    });
    this.pager = this.pagerservice.getPager(this.pagedItems.length, 1, 8);
    this.pagedItems = this.pagedItems.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  setPage(page: number) {
    if (page < 1 || page > this.pager.totalPages) {
      return;
    }
    this.pager = this.pagerservice.getPager(this.listaFileDCM.length, page, 8);

    // pega a página atual de itens
    this.pagedItems = this.listaFileDCM.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  Notas(fileDCM: FileDCMComponent) {
    fileDCM.countNota = 0;
    const activeModal = this.modalService.open(WorklistNotasComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.modalHeader = fileDCM.studyDesc;
    activeModal.componentInstance.fileDCM = fileDCM;
  }

  Anexos(fileDCM: FileDCMComponent) {
    const activeModal = this.modalService.open(WorklistAnexosComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.fileDCM = fileDCM;
  }

  SelecionarTemplateImpressao(fileDCM: FileDCMComponent) {
    this.templateImpressaoService.ListaComboByFacility(fileDCM.facilityId).subscribe(
      listaTemplateImpressao => {
        if (!listaTemplateImpressao) {
          console.log("Nenhum template de impressão localizado.");
        } else {
          if (listaTemplateImpressao.length > 1) {
            const activeModal = this.modalService.open(WorklistTemplateImpressaoComponent, {
              size: "lg",
              backdrop: "static",
              container: "nb-layout"
            });
            activeModal.componentInstance.modalHeader = fileDCM.studyDesc;
            activeModal.componentInstance.fileDCM = fileDCM;
          } else {
            this.GetLaudoPage(fileDCM.Id, listaTemplateImpressao[0].Id)
          }
        }
      },
      erro => {
        this.templateImpressaoService.erro(erro);
      }
    );
  }

  Copylaudo(fileDCMId: string){
    this.passEntry.emit(fileDCMId);
    this.closeModal();
  }

  GetLaudoPage(fileDCMId, templateImpressaoid) {
    if (templateImpressaoid && fileDCMId) {
      window.open(`./#/laudopdf/${fileDCMId}_${templateImpressaoid}`, "laudopdf", 'width=1200,height=900');
    } else {
      console.log("Não foi possível gerar laudo!");
      this.templateImpressaoService.showToast(
        "error",
        "Não foi possível gerar laudo!"
      );
    }
  }
}
