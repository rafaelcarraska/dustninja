import { EChartOption } from 'echarts';
import { ExameService } from './../../../@core/data/exame.service';
import { HttpClient } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbActiveModal, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FileDCMComponent } from '../../../@core/model/fileDCM/fileDCM.component';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  moduleId: module.id,
  selector: "./worklist-estatisticas",
  templateUrl: "./worklist-estatisticas.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./worklist-estatisticas.component.scss"
  ]
})
export class WorklistEstatisticaComponent {
  @Input()
  modalHeader: string = "";
  exameService: ExameService;

  errors: string[];
  erroNovaDescricao: string;

  totalItens: number;
  totalItensLaudados: number;
  totalItensLaudar: number;
  totalItensConfimar: number;
  totalItensDesconciderados: number;
  totalItensComparacao: number;
  totalItensTrasmissao: number;

  constructor(
    private route: ActivatedRoute,
    private readonly router: Router,
    private activeModal: NgbActiveModal,
    exameService: ExameService,
    private modalService: NgbModal,
    private http: HttpClient
  ) {
    this.exameService = exameService;
  }

  chartOption: EChartOption = {};

  ngOnInit() {
    this.chartOption = {
      title: {
        text: 'Total de exames',
        subtext: this.totalItens,
        x: 'center'
      },
      tooltip: {
        trigger: 'item',
        formatter: "{a} <br/>{b} : {c} ({d}%)"
      },
      legend: {
        type: 'scroll',
        orient: 'vertical',
        right: 10,
        top: 40,
        bottom: 20,
        data: ["Laudados", "Laudar", "Desconciderados", "Confimar", "Comparacao", "Trasmissão"],
        selected: ["Laudar", "Desconciderados", "Confimar", "Comparacao", "Trasmissão"]
      },
      toolbox: {
        right: 50,
        show: true,
        feature: {
          mark: { show: true },
          dataView: { show: false, readOnly: true, title: 'Informações'},
          saveAsImage: { show: true, title: 'Download' }
        }
      },
      series: [
        {
          type: 'pie',
          radius: '55%',
          center: ['40%', '50%'],
          data: [{
            "name": "Laudados",
            "value": this.totalItensLaudados
          },
          {
            "name": "Laudar",
            "value": this.totalItensLaudar
          },
          {
            "name": "Confimar",
            "value": this.totalItensConfimar
          },
          {
            "name": "Desconciderados",
            "value": this.totalItensDesconciderados
          },
          {
            "name": "Comparacao",
            "value": this.totalItensComparacao
          },
          {
            "name": "Trasmissão",
            "value": this.totalItensTrasmissao
          }],
          itemStyle: {
            emphasis: {
              shadowBlur: 10,
              shadowOffsetX: 0,
              shadowColor: 'rgba(0, 0, 0, 0.5)'
            }
          }
        }
      ]
    }
  }

  closeModal() {
    this.activeModal.close();
  }
}
