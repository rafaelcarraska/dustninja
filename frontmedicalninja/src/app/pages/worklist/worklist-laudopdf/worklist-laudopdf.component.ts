import { WorklistService } from './../../../@core/data/worklist.service';
import { Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { StateService } from '../../../@core/data/state.service';
import { LaudoPageComponent } from '../../../@core/model/laudoPage/laudoPage.component';
import * as jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { saveAs } from 'file-saver';
import { ExameService } from '../../../@core/data/exame.service';

@Component({
  moduleId: module.id,
  selector: "worklistLaudoPDF",
  templateUrl: "./worklist-laudopdf.component.html",
  styleUrls: ["./worklist-laudopdf.component.scss"]
})
export class LaudoPDFComponent {
  laudoPage: LaudoPageComponent = new LaudoPageComponent();
  worklistService: WorklistService;
  exameService: ExameService;
  load: boolean = false;
  facilityId: string;
  loadpdf: boolean = false;

  constructor(
    protected stateService: StateService,
    private route: ActivatedRoute,
    worklistService: WorklistService,
    exameService: ExameService
  ) {
    this.worklistService = worklistService;
    this.exameService = exameService;
    this.laudoPage.header = "Carregando...";
    this.stateService.setpopUpState(false);
    this.route.params.subscribe(params => this.CarregaLaudoPage(params.id));
  }

  CarregaLaudoPage(facilityId: string){
    this.load = true;
    let fileDCMId = facilityId.split("_")[0];
    let logView = this.exameService.montaLogVisualizacao('laudo', fileDCMId);
    this.exameService.LogVisualizacao(logView).subscribe();
    this.facilityId = facilityId;
    this.worklistService.GetLaudoPage(fileDCMId).subscribe(
      laudoPage => {
        if (!laudoPage){
        this.worklistService.showToast(
            "error",
            "Não foi possível o laudo!"
          );
        }else{
          this.laudoPage = laudoPage;
        }
      },
      erro => {
        this.laudoPage.header = "Erro ao carregar o laudo.";
        this.worklistService.erro(erro);
      },
      () =>{
        this.load = false;
      }
    );
  }

  DownloadpdfCanvas(){
    let doc = new jsPDF();

    let element = <HTMLScriptElement>document.getElementsByClassName("pagePDF")[0];
    html2canvas(element)
    .then((canvas: any) => {
      doc.addImage(canvas.toDataURL("image/jpeg"), "JPEG", 0, 0,
      doc.internal.pageSize.width, element.offsetHeight / 5 );
      doc.save(`Laudo-${Date.now()}.pdf`);
    })
  }

  GetLaudoPage(fileDCMId: string){
    let logView = this.exameService.montaLogVisualizacao('pdf', fileDCMId);
    this.exameService.LogVisualizacao(logView).subscribe();
    this.worklistService.GetLaudoPage(fileDCMId).subscribe(
      laudoPage => {
        if (!laudoPage){
          this.worklistService.showToast(
            "error",
            "Não foi possível carregar o laudo!"
          );
        }else{
          this.laudoPage = laudoPage;
        }
      },
      erro => {
        this.laudoPage.header = "Erro ao carregar o laudo.";
        this.worklistService.erro(erro);
      }
    );
  }

  ServiceWindowsNinja(){
    this.loadpdf = true;
    let fileDCMId = this.facilityId.split("_")[0];
    let logView = this.exameService.montaLogVisualizacao('pdf', fileDCMId);
    this.exameService.LogVisualizacao(logView).subscribe();
    let filename = `laudoFastPacs.pdf`;
    //parametros das configuracoes da paginacao
    this.laudoPage.floatx = 250;
    this.laudoPage.floaty = 10;
    this.laudoPage.floatwidth = 300;
    this.laudoPage.floateheight = 10;
    this.worklistService.ServiceWindowsNinja(this.laudoPage).subscribe(
      data => {
        saveAs(data, filename);
      },
      err => {
        this.worklistService.showToast(
          "error",
          "Não foi possivel realizar o download!"
        );
        console.error(err);
      },
      () =>{
        this.loadpdf = false;
      }
    );
  }

  ShowErrors(errors){
    if (errors) {
      errors.forEach(erro => {
        this.worklistService.showToast("warning", erro);
      });
    }
  }

}
