import { WorklistEstatisticaComponent } from './worklist-estatisticas/worklist-estatisticas.component';
import { ConfiguracaoComponent } from './../../@core/model/configuracao/configuracao.component';
import { ConfiguracaoService } from './../../@core/data/configuracao.service';
import { Component, ViewChild } from "@angular/core";
import { WorklistService } from "../../@core/data/worklist.service";
import { FileDCMComponent } from "../../@core/model/fileDCM/fileDCM.component";
import { ToasterConfig } from "angular2-toaster";
import { WorklistNotasComponent } from "./worklist-notas/worklist-notas.component";
import { NgbModal, NgbTabTitle } from "@ng-bootstrap/ng-bootstrap";
import { PagerService } from "../../@core/data/pager.service";
import { WorklistAnexosComponent } from "./worklist-anexos/worklist-anexos.component";
import { WorklistLogComponent } from "./worklist-log/worklist-log.component";
import { WorklistConfirmacaoComponent } from "./worklist-confirmacao/worklist-confirmacao.component";
import { ExameService } from '../../@core/data/exame.service';
import { NbSidebarService } from '@nebular/theme';
import { environment } from '../../../environments/environment';
import { ConfirmacaoComponent } from '../../@core/model/fileDCM/confirmacao.component';
import { WorklistReiterpretacaoComponent } from './worklist-reinterpretacao/worklist-reinterpretacao.component';
import { saveAs } from 'file-saver';
import { WorklistTemplateImpressaoComponent } from './worklist-templateImpressao/worklist-templateImpressao.component';
import { TemplateImpressaoService } from '../../@core/data/templateImpressao.service';
import { WorklistStatusComponent } from './worklist-status/worklist-status.component';
import { WorklistRoubarLaudoComponent } from './worklist-roubarLaudo/worklist-roubarLaudo.component';
import { ComboboxComponent } from '../../@core/model/combobox/combobox.component';
import { FiltrosService } from '../../@core/data/filtros.service';
import { ActivatedRoute, Router } from '@angular/router';
import { stringify } from '@angular/core/src/util';
import { ContextMenuComponent } from 'ngx-contextmenu';
import { WorklistDesmembrarComponent } from './worklist-desmembrar/worklist-desmembrar.component';
import { WorklistExamesAnterioresComponent } from './worklist-examesanteriores/worklist-examesanteriores.component';
import { NotaHistoricoComponent } from '../../@core/model/NotaHistorico/notaHistorico.component';


@Component({
  selector: "worklist",
  templateUrl: "./worklist.component.html",
  styleUrls: ["./worklist.component.scss"]
})
export class WorklistComponent {
  listaNotas: NotaHistoricoComponent[];
  listaFiltro: ComboboxComponent[] = [];
  filtroPage: string = "";
  public searchString: string;
  listaFileDCM: FileDCMComponent[] = [];
  configuracao: ConfiguracaoComponent;
  exibirDivConfiguracoes: boolean = false;
  load: boolean = false;
  worklistservice: WorklistService;
  pagerservice: PagerService;
  configuracaoservice: ConfiguracaoService;
  exameService: ExameService;
  serviceUrlDCM: string = environment.serviceUrlDCM;
  serviceUrlOsirix: string = environment.serviceUrlOsirix;
  serviceUrlHorus: string = environment.serviceUrlHorus;
  serviceUrlOHIF: string = environment.serviceUrlOHIF;
  viewOsirix: boolean = environment.viewOsirix;
  viewHoros: boolean = environment.viewHoros;
  // permitirDownload: boolean = false;
  beta: boolean = environment.beta;
  confirmacao: ConfirmacaoComponent = new ConfirmacaoComponent();
  templateImpressaoService: TemplateImpressaoService;
  filtrosservice: FiltrosService;
  filtroSelecionado: string;

  @ViewChild(ContextMenuComponent) public basicMenu: ContextMenuComponent;
  NotaHistoricoComponent: any;


  constructor(
    private route: ActivatedRoute,
    private readonly router: Router,
    private sidebarService: NbSidebarService,
    worklistservice: WorklistService,
    private modalService: NgbModal,
    pagerservice: PagerService,
    configuracaoservice: ConfiguracaoService,
    exameService: ExameService,
    templateImpressaoService: TemplateImpressaoService,
    filtrosservice: FiltrosService,
  ) {
    this.filtrosservice = filtrosservice;
    this.worklistservice = worklistservice;
    this.pagerservice = pagerservice;
    this.configuracaoservice = configuracaoservice;
    this.exameService = exameService;
    this.templateImpressaoService = templateImpressaoService;
    // this.collapseSidebar();
  }
  config: ToasterConfig;

  pager: any = {};
  pagedItems: any[];
  filtroItems: any[];

  ngAfterViewInit() {
    this.BuscarConfiguração();
    // this.filtroSelecionado = "0";
    // console.log('platform', navigator.platform);
    if(navigator.platform.toLowerCase() == 'macintel' && this.beta){
      this.viewOsirix = true;
      this.viewHoros = true;
    }
  }

  dbfiltro(texto: string){
    this.searchString = texto;
    this.setfiltro();
  }

  setPage(page: number, orderBy: string = '') {
    if (page < 1 || page > this.pager.totalPages) {
        return;
    }

    // pega o objeto pager do serviço
    if(this.configuracao.pageSize <= 0){
      this.configuracao.pageSize = 10;
    }
    if (this.searchString) {
      this.pager = this.pagerservice.getPager(this.filtroItems.length, page, this.configuracao.pageSize);
      this.pagedItems = this.filtroItems.slice(this.pager.startIndex, this.pager.endIndex + 1);
    }else{
      this.pager = this.pagerservice.getPager(this.listaFileDCM.length, page, this.configuracao.pageSize);
      this.pagedItems = this.listaFileDCM.slice(this.pager.startIndex, this.pager.endIndex + 1);
    }
  }

  ExamesAnteriores(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(WorklistExamesAnterioresComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout",
      windowClass : "modalxxl"
    });
    activeModal.componentInstance.fileDCM = fileDCM;
    activeModal.componentInstance.copylaudo = false;
  }

  SelecionaFiltro(filtro: string){
    this.searchString = '';
    this.filtroSelecionado = filtro;
    this.AtualizaGrid();
  }

  LoadFiltro() {
    this.filtrosservice.listaCombo().subscribe(
      listaFiltro => {
        if (!listaFiltro){
          console.log("Nenhum filtro localizado.");
        }
        this.listaFiltro = listaFiltro;
      },
      erro =>  {
        this.filtrosservice.erro(erro);
        this.LoadGrid();
      },
      () =>{
        this.filtroSelecionado = this.configuracao.filtroPadrao;
        this.AtualizaGrid();
      }
    );
  }

  setfiltro() {
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

    this.filtroItems = this.listaFileDCM.filter(item => {
      return filterKeys.some(keyName => {
        return (
          new RegExp(filter[keyName], "gi").test(item[keyName]) || filter[keyName] == "");
      });
    });
    this.pagedItems = this.filtroItems;
    this.pager = this.pagerservice.getPager(this.pagedItems.length, 1, this.configuracao.pageSize);
    this.pagedItems = this.pagedItems.slice(this.pager.startIndex, this.pager.endIndex + 1);

    if (!this.searchString || !Array.isArray(this.listaFileDCM)) {
      this.setPage(1);
      return;
    }
  }

  DownloadDICOM(fileDCM: FileDCMComponent){
    if(!fileDCM.permitirDownload){
      this.worklistservice.showToast("warning", "Aceso negado.");
    }
    let filename = `${fileDCM.paciente.nomeCompleto}.zip`;
    this.configuracaoservice.showToast(
      "success",
      "Estamos processando o arquivo, Aguarde!"
    );
    this.load = true;
    this.worklistservice.DownloadDICOM(`${this.serviceUrlDCM}dcm4chee-arc/aets/DCM4CHEE/rs/studies/${fileDCM.studyId}?accept=application/zip`).subscribe(
      data => {
        saveAs(data, filename);
      },
      err => {
        this.worklistservice.showToast(
          "error",
          "Não foi possivel realizar o download!"
        );
        console.error(err);
      },
      () =>{
        this.load = false;
      }
    );
    // window.open(`${this.serviceUrlDCM}dcm4chee-arc/aets/DCM4CHEE/rs/studies/${fileDCM.studyId}?accept=application/zip`);
  }

  ViewOsirix(fileDCM: FileDCMComponent){
    window.open(`${this.serviceUrlOsirix}${this.serviceUrlDCM}dcm4chee-arc/aets/DCM4CHEE/rs/studies/${fileDCM.studyId}?accept=application/zip`);
  }

  ViewHoros(fileDCM: FileDCMComponent){
    window.open(`${this.serviceUrlHorus}${this.serviceUrlDCM}dcm4chee-arc/aets/DCM4CHEE/rs/studies/${fileDCM.studyId}?accept=application/zip`);
  }

  AbrirModalDesmembrar(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(WorklistDesmembrarComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.fileDCM = fileDCM;
    activeModal.componentInstance.confirmacao = false;
  }

  AlterarExame(fileDCM: FileDCMComponent){
    this.router.navigate([`/pages/exame/${fileDCM.Id}`]);
  }

  BuscarConfiguração(){
    this.configuracaoservice.lista().subscribe(
      configuracao => {
        if (!configuracao){
          this.configuracao.pageSize = 10;
          if(!this.configuracao.filtroPadrao){
            this.configuracao.filtroPadrao = "0";
          }else{
            this.configuracao.filtroPadrao = configuracao.filtroPadrao;
          }
        }else{
          this.configuracao = configuracao;
        }
      },
      erro => {
        this.configuracaoservice.erro(erro);
        this.configuracao.pageSize = 10;
      },
      () => {
        this.LoadFiltro();
      }
    );
  }

  VerificaFiltroPage(){
    let filtroPage: string = "";
    this.route.params.subscribe(
      params => {
        filtroPage = params.id || "";
        this.filtroPage = filtroPage;
    });
    // console.log('filtroPage', filtroPage);
  }

  LoadGrid() {
    this.VerificaFiltroPage();

    this.load = true;
    this.worklistservice.lista(this.filtroPage).subscribe(
      listaFileDCM => {
        this.listaFileDCM = listaFileDCM;
        this.setfiltro();
        // console.log(listaFileDCM);
      },
      erro => {
        this.worklistservice.erro(erro);
      },
      () =>{
        this.load = false;
      }
    );
  }

  LoadGridFiltro(filtroId: string) {
    this.load = true;
    this.worklistservice.listaFiltro(filtroId).subscribe(
      listaFileDCM => {
        this.listaFileDCM = listaFileDCM;
        // this.setPage(1);
        this.setfiltro();
        // console.log(listaFileDCM);
      },
      erro => {
        this.worklistservice.erro(erro);
      },
      () =>{
        this.load = false;
      }
    );
  }

  SelecionarTemplateImpressao(fileDCM: FileDCMComponent){
    this.templateImpressaoService.ListaComboByFacility(fileDCM.facilityId).subscribe(
      listaTemplateImpressao => {
        if (!listaTemplateImpressao){
          console.log("Nenhum template de impressão localizado.");
        }else{
          if(listaTemplateImpressao.length > 1){
            const activeModal = this.modalService.open(WorklistTemplateImpressaoComponent, {
              size: "lg",
              backdrop: "static",
              container: "nb-layout"
            });
            activeModal.componentInstance.modalHeader = fileDCM.studyDesc;
            activeModal.componentInstance.fileDCM = fileDCM;
          }else{
            this.GetLaudoPage(fileDCM.Id, listaTemplateImpressao[0].Id)
          }
        }
      },
      erro => {
        this.templateImpressaoService.erro(erro);
      }
    );
  }

  GetLaudoPage(fileDCMId, templateImpressaoid){
    if(templateImpressaoid && fileDCMId){
      window.open(`./#/laudopdf/${fileDCMId}_${templateImpressaoid}`, "laudopdf", 'width=1200,height=900');
    }else{
      console.log("Não foi possível gerar laudo!");
      this.templateImpressaoService.showToast(
        "error",
        "Não foi possível gerar laudo!"
      );
    }
  }

  Anexos(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(WorklistAnexosComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.fileDCM = fileDCM;
  }

  Laudo(fileDCM: FileDCMComponent){
    this.VerificaStatusExame(fileDCM);
  }

  AbrirConfirmacao(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(WorklistConfirmacaoComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.modalHeader = fileDCM.studyDesc;
    activeModal.componentInstance.fileDCM = fileDCM;
  }

  OpenOhif(fileDCM: FileDCMComponent){
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

  downloadFileDCM(fileDCMId: string){
    let logView = this.exameService.montaLogVisualizacao('download', fileDCMId);
    this.exameService.LogVisualizacao(logView).subscribe();
  }

  AbrirReinterpretacao(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(WorklistReiterpretacaoComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.modalHeader = fileDCM.studyDesc;
    activeModal.componentInstance.fileDCM = fileDCM;
  }

  Laudar(fileDCM: FileDCMComponent){
    if(fileDCM.permitirLaudo){
      // console.log('Laudar', fileDCM);
      this.confirmacao.fileDCMId = fileDCM.Id;
      this.confirmacao.prioridade = fileDCM.prioridade;
      this.confirmacao.statusExames = "laudando";
      this.confirmacao.subStatusExames = fileDCM.subStatusExamesFormatado;

      this.exameService.Confirmacao(this.confirmacao).subscribe(
        msg => {
          if (!msg.erro) {
            fileDCM.statusExamesFormatado = "laudando";
            window.open(`./#/pages/laudo/${fileDCM.Id}`, "laudo", 'width=1200,height=900');
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

  collapseSidebar() {
    this.sidebarService.collapse('menu-sidebar');
  }

  VerificaStatusExame(fileDCM: FileDCMComponent){
    this.exameService.VerificaStatusExame(fileDCM.Id).subscribe(
      statusExame => {
        if (!statusExame){
          this.exameService.showToast(
            "error",
            "Não foi possível verificar o status do exame!"
          );
        }else{
          // console.log('status do exame', statusExame);
          switch (statusExame.statusExamesFormatado) {
            case  'confirmar':
              this.VerificaStatusExameConfirmar(fileDCM);
              break;
            case 'laudar':
              this.VerificaStatusExameLaudar(fileDCM);
              break;
            case 'laudando':
              this.VerificaStatusExameLaudando(fileDCM, statusExame);
              break;
            case 'laudado':
              this.AbrirReinterpretacao(fileDCM);
              break;
            case 'desconsiderado':
              this.VerificaStatusExameAlterarStatus(fileDCM);
              break;
            case 'comparacao':
              this.VerificaStatusExameAlterarStatus(fileDCM);
              break;
          }
        }
      },
      erro => {
        this.exameService.erro(erro);
      }
    );
  }

  VerificaStatusExameConfirmar(fileDCM: FileDCMComponent){
    if (!fileDCM.confirmarExame){
      this.exameService.showToast(
        "warning",
        "Você não possui permissão para confirmar esse exame!"
      );
      return;
    }
    this.AbrirConfirmacao(fileDCM);
  }

  VerificaStatusExameLaudar(fileDCM: FileDCMComponent){
    if(fileDCM.permitirLaudo && fileDCM.confirmarExame){
      this.AlterarStatus(fileDCM);
      return;
    }else if(fileDCM.confirmarExame){
      this.AbrirConfirmacao(fileDCM);
      return;
    }else if(fileDCM.permitirLaudo){
      this.Laudar(fileDCM);
      return;
    }
    this.exameService.showToast(
      "warning",
      "Você não possui permissão para confirmar ou laudar esse exame!"
    );
  }

  VerificaStatusExameLaudando(fileDCM: FileDCMComponent, statusExame: any){
    if(!fileDCM.permitirLaudo){
      this.exameService.showToast(
        "warning",
        "Você não possui permissão para laudar esse exame!"
      );
      return;
    }
    if(statusExame.usuarioLogado){
      this.Laudar(fileDCM);
    }else{
      const activeModal = this.modalService.open(WorklistRoubarLaudoComponent, {
        size: "lg",
        backdrop: "static",
        container: "nb-layout"
      });
      activeModal.componentInstance.fileDCM = fileDCM;
      activeModal.componentInstance.confirmacao = this.confirmacao;
    }
  }

  VerificaStatusExameAlterarStatus(fileDCM: FileDCMComponent){
    if(fileDCM.confirmarExame){
      this.AbrirConfirmacao(fileDCM);
      return;
    }
    this.exameService.showToast(
      "warning",
      "Você não possui permissão para confirmar ou laudar esse exame!"
    );
  }

  AlterarStatus(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(WorklistStatusComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.modalHeader = fileDCM.studyDesc;
    activeModal.componentInstance.fileDCM = fileDCM;
    activeModal.componentInstance.confirmacao = this.confirmacao;
  }

  Log(fileDCM: FileDCMComponent){
    const activeModal = this.modalService.open(WorklistLogComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.modalHeader = fileDCM.studyDesc;
    activeModal.componentInstance.fileDCM = fileDCM;
  }


  LoadNotas() {
    this.NotaHistoricoComponent.listaNotas().subscribe(
      listaNotas => {
        if (listaNotas) {
          this.listaNotas = listaNotas;
        }else{
          this.listaNotas = [];
        }
        this.NotaHistoricoComponent.countNota = this.NotaHistoricoComponent.length;
      },
      erro => {
        console.log(erro);
        this.worklistservice.erro(erro);
      }
    );
  }

  Notas(fileDCM: FileDCMComponent){
    fileDCM.countNota = 0;
    const activeModal = this.modalService.open(WorklistNotasComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.modalHeader = fileDCM.studyDesc;
    activeModal.componentInstance.fileDCM = fileDCM;
  }

  Configuracao(){
    this.exibirDivConfiguracoes = !this.exibirDivConfiguracoes;
  }

  SalvarConfiguracao(){
    this.configuracaoservice.Salvar(this.configuracao).subscribe(
      msg => {
        if (!msg.erro) {
          if (!this.configuracao.Id && msg.id) {
            this.configuracao.Id = msg.id;
          }
          this.configuracaoservice.showToast(
            "success",
            "Configuracao salva com sucesso!"
          );
          this.exibirDivConfiguracoes = false;
        } else {
          this.ShowErrors(msg.erro);
        }
      },
      erro => {
        this.configuracaoservice.showToast(
          "error",
          "Não foi possível salvar a configuracao!"
        );
      }
    );
    this.setPage(1);
  }

  refresh(){
    this.VerificaFiltroPage();

    if(this.filtroPage != '' && this.filtroPage != undefined){
      this.router.navigate([`/pages/worklist`]);
    }else{
      this.AtualizaGrid();
    }
  }

  AtualizaGrid(){
    this.VerificaFiltroPage();

    // console.log("filtroSelecionado", this.filtroSelecionado);
    if((this.filtroPage != '' && this.filtroPage != undefined) || this.filtroSelecionado == "0" || !this.filtroSelecionado || this.filtroSelecionado == undefined){
      this.LoadGrid();
    }else{
      this.LoadGridFiltro(this.filtroSelecionado);
    }
  }

  seguir(fileDCM: FileDCMComponent)
  {
    this.worklistservice.favoritar(fileDCM.Id).subscribe(
      msg => {
        if (!msg.erro) {
          fileDCM.favorito = !fileDCM.favorito;

          this.worklistservice.showToast("success", "Agora você receberar notificações desse exame!");
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
          "Não foi possível favoritar esse exame!"
        );
      }
    );
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.worklistservice.showToast("warning", erro);
      });
    }
  }

  FiltraPaciente(fileDCM: FileDCMComponent){
    this.router.navigate([`/pages/worklist/paciente:${fileDCM.paciente.Id}`]);
  }

  PagePaciente(fileDCM: FileDCMComponent){
    this.router.navigate(["/pages/paciente/"+fileDCM.paciente.Id]);
  }

  Estatisticas(){
    const activeModal = this.modalService.open(WorklistEstatisticaComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.totalItens = this.listaFileDCM.length;
    activeModal.componentInstance.totalItensLaudados = this.listaFileDCM.filter(x => x.statusExamesFormatado == 'laudado').length;
    activeModal.componentInstance.totalItensLaudar = this.listaFileDCM.filter(x => x.statusExamesFormatado == 'laudar').length;
    activeModal.componentInstance.totalItensConfimar = this.listaFileDCM.filter(x => x.statusExamesFormatado == 'confirmar').length;
    activeModal.componentInstance.totalItensDesconciderados = this.listaFileDCM.filter(x => x.statusExamesFormatado == 'desconsiderado').length;
    activeModal.componentInstance.totalItensComparacao = this.listaFileDCM.filter(x => x.statusExamesFormatado == 'comparacao').length;
    activeModal.componentInstance.totalItensTrasmissao = this.listaFileDCM.filter(x => x.statusExamesFormatado == 'transmissao').length;
  }
}
