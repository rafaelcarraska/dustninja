import { ExameService } from './../../../@core/data/exame.service';
import { TipoEstudoComponent } from './tipoEstudo/tipoEstudo.component';
import { ConfirmacaoComponent } from './../../../@core/model/fileDCM/confirmacao.component';
import { WorklistService } from './../../../@core/data/worklist.service';
import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FileDCMComponent } from "../../../@core/model/fileDCM/fileDCM.component";
import { MascaraLaudoService } from '../../../@core/data/mascaraLaudo.service';
import { MascaraLaudoComponent } from '../../mascaraLaudo/mascaraLaudo.component';
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { StateService } from '../../../@core/data/state.service';
import { ComboboxComponent } from '../../../@core/model/combobox/combobox.component';
import { TipoExameService } from '../../../@core/data/tipoExame.service';
import { NotaHistoricoComponent } from '../../../@core/model/NotaHistorico/notaHistorico.component';
import { HistoricoClinicoComponent } from './historicoClinico/historicoClinico.component';
import { WorklistNotasComponent } from '../worklist-notas/worklist-notas.component';
import { environment } from '../../../../environments/environment';
import { WorklistAnexosComponent } from '../worklist-anexos/worklist-anexos.component';
import { WorklistExamesAnterioresComponent } from '../worklist-examesanteriores/worklist-examesanteriores.component';
import { RevisorComponent } from './revisor/revisor.component';

@Component({
  moduleId: module.id,
  selector: "worklistLaudo",
  templateUrl: "./worklist-laudo.component.html",
  styleUrls: ["./worklist-laudo.component.scss"]
})
export class LaudoComponent {
  listaMascara: MascaraLaudoComponent[] = [];
  ckeConfig: any;
  fileDCM: FileDCMComponent = new FileDCMComponent();
  confirmacao: ConfirmacaoComponent = new ConfirmacaoComponent();
  worklistService: WorklistService;
  mascaraLaudoservice: MascaraLaudoService;
  tipoExameservice: TipoExameService;
  exameService: ExameService;
  listatipoEstudo: ComboboxComponent[];
  textoBotao: string = "";
  notaResumida: string = "";
  historicoClinico: NotaHistoricoComponent = new NotaHistoricoComponent();
  load: boolean = true;
  serviceUrlDCM: string = environment.serviceUrlDCM;
  serviceUrlOHIF: string = environment.serviceUrlOHIF;
  msgError: string = "";

  constructor(
    protected stateService: StateService,
    private route: ActivatedRoute,
    private readonly router: Router,
    private modalService: NgbModal,
    worklistService: WorklistService,
    mascaraLaudoservice: MascaraLaudoService,
    tipoExameservice: TipoExameService,
    exameService: ExameService,
  ) {

    this.worklistService = worklistService;
    this.mascaraLaudoservice = mascaraLaudoservice;
    this.tipoExameservice = tipoExameservice;
    this.exameService = exameService;

    this.stateService.setpopUpState(false);

    // window.addEventListener('beforeunload', function (e) {
    //   // Cancel the event
    //   e.preventDefault();
    //   // Chrome requires returnValue to be set
    //   e.returnValue = '';
    // });

    this.route.params.subscribe(params => this.CarregarExame(params.id));
  }

  ngOnInit() {
    this.ckeConfig = {
      height: '445px',
      width: '780px',
      styles: { 'color': '#000000' },
      allowedContent: false,
      forcePasteAsPlainText: true,
      editorConfig: 'pt-br',
      removePlugins: 'placeholder_select, placeholder',
      toolbarGroups: [
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
        { name: 'clipboard', groups: ['clipboard', 'undo'] },
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
        { name: 'colors', groups: ['colors'] },
        { name: 'tools', groups: ['tools'] },
        { name: 'document', groups: ['mode', 'document', 'doctools'] },
        { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
        { name: 'forms', groups: ['forms'] },
        { name: 'links', groups: ['links'] },
        { name: 'insert', groups: ['insert'] },
        { name: 'styles', groups: ['styles'] },
        { name: 'others', groups: ['others'] },
        { name: 'about', groups: ['about'] }
      ],
      removeButtons: 'Save,Templates,NewPage,Preview,Print,SelectAll,Scayt,Form,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,CopyFormatting,NumberedList,BulletedList,Outdent,Indent,Blockquote,CreateDiv,BidiLtr,BidiRtl,Language,Anchor,Unlink,Link,Image,Flash,Table,HorizontalRule,Smiley,SpecialChar,PageBreak,Iframe,Styles,Format,Font,FontSize,ShowBlocks,About,Source,Find,Replace,PasteText,PasteFromWord,BGColor,TextColor'
    };
  }

  CarregarExame(fileDCMId: string) {
    this.worklistService.listaExame(fileDCMId).subscribe(
      fileDCM => {
        if (!fileDCM) {
          this.DivError("Não foi possível buscar o exame!");
          console.log("Não foi possível buscar o exame!");
        } else {
          console.log(fileDCM);
          this.fileDCM = fileDCM;
          this.HistoricoClinicoLoad(this.fileDCM);
        }
      },
      erro => {
        console.log(erro);
        this.worklistService.erro(erro);
      }
    );
  }

  DivError(msg: string) {
    this.msgError = msg;
    setTimeout(() => {
      this.msgError = "";
    },
      10000);
  }

  CarregaUltimoLaudo(fileDCMId: string) {
    this.exameService.CarregaUltimoLaudo(fileDCMId).subscribe(
      confirmacao => {
        if (!confirmacao) {
          this.DivError("Não foi possível buscar ultmiolaudo!");
          console.log("Não foi possível buscar ultmiolaudo!");
        } else {
          console.log('CarregaUltimoLaudo', confirmacao);
          this.confirmacao = confirmacao;
          this.listatipoEstudo.forEach(tpEstudo => {
            if (this.confirmacao.listaTipoEstudo.filter(x => x == tpEstudo.Id).length > 0) {
              tpEstudo.status = true;
            }
          });
          this.AtualizaBotao();
        }
      },
      erro => {
        console.log(erro);
        this.exameService.erro(erro);
      }
    );
  }

  HistoricoClinicoLoad(fileDCM: FileDCMComponent) {
    this.exameService.HistoricoClinicoLoad(fileDCM.Id).subscribe(
      historicoClinico => {
        if (!historicoClinico) {
          this.DivError("Nenhuma histórico clínico localizado.");
          console.log("Nenhuma histórico clínico localizado.");
        } else {
          this.historicoClinico = historicoClinico;
          if (this.historicoClinico.nota) {
            if (this.historicoClinico.nota.length > 120) {
              this.notaResumida = this.historicoClinico.nota.substring(0, 120) + ' ...';
            } else {
              this.notaResumida = this.historicoClinico.nota ? this.historicoClinico.nota : "";
            }
          }
          this.OpenHistorico();
        }
      },
      erro => {
        this.exameService.erro(erro);
      },
      () => {
        if (this.fileDCM.subStatusExamesFormatado == 'laudar_reiterpretacao') {
          this.CarregaUltimoLaudo(fileDCM.Id);
        }
        this.LoadMascara(this.fileDCM.Id);
        this.LoadGridTipoEstudo(this.fileDCM.modality);
      }
    );
  }

  LoadMascara(fileDcmId: string) {
    this.mascaraLaudoservice.listaOrderbyModalidade(fileDcmId).subscribe(
      listaMascara => {
        if (!listaMascara) {
          this.DivError("Nenhuma Máscara de Laudo localizada.");
          console.log("Nenhuma Máscara de Laudo localizada.");
        }
        this.listaMascara = listaMascara;
      },
      erro => {
        this.mascaraLaudoservice.erro(erro);
      }
    );
  }

  SelecionaMascara(mascaraLaudo: MascaraLaudoComponent, myckeditor: any) {
    myckeditor.instance.insertHtml(mascaraLaudo.laudo);
  }

  OpenHistorico() {
    const activeModal = this.modalService.open(HistoricoClinicoComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.historicoClinico = this.historicoClinico;
  }

  OpenOhif(fileDCM: FileDCMComponent) {
    let iframe = `<html><head>
    <meta charset="utf-8"><title>Fastpacs</title>
    <style>body, html {width: 100%; height: 100%; margin: 0; padding: 0}</style>
    </head><body>
    <iframe src="${this.serviceUrlOHIF}viewer/${fileDCM.studyId}" style="height:calc(100% - 4px);width:calc(100% - 4px)"></iframe>
    </html></body>`;

    let win = window.open('', "Ohif", 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,resizable=no');
    win.document.body.innerHTML = iframe;
  }

  SelecionarTipoEstudo(botaoAssinar: boolean = false) {
    const activeModal = this.modalService.open(TipoEstudoComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.listatipoEstudo = this.listatipoEstudo;
    activeModal.componentInstance.botaoAssinar = botaoAssinar;
    activeModal.componentInstance.passEntry.subscribe((receivedEntry) => {
      if (receivedEntry) {
        this.Assinar();
      }
    });
    activeModal.result.then(() => {
      this.AtualizaBotao();
    });
  }

  ExamesAnteriores(fileDCM: FileDCMComponent) {
    const activeModal = this.modalService.open(WorklistExamesAnterioresComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout",
      windowClass: "modalxxl"
    });
    activeModal.componentInstance.fileDCM = fileDCM;
    activeModal.componentInstance.copylaudo = true;
    activeModal.componentInstance.passEntry.subscribe((receivedEntry) => {
      this.CarregaUltimoLaudo(receivedEntry);
    });
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

  AtualizaBotao() {
    this.textoBotao = this.listatipoEstudo
      .filter(x => x.status == true)
      .map(({ descricao }) => descricao)
      .join(', ');
  }

  LoadGridTipoEstudo(modalityFileDCM: string) {
    this.tipoExameservice.listaTipoEstudo(modalityFileDCM).subscribe(
      listatipoEstudo => {
        if (!listatipoEstudo) {
          this.DivError("Nenhum TipoExame localizado.");
          console.log("Nenhum TipoExame localizado.");
        }
        this.listatipoEstudo = listatipoEstudo;
        this.load = false;
      },
      erro => {
        this.tipoExameservice.erro(erro);
      }
    );
  }

  closeWindows() {
    if (window.confirm("Deseja fechar a janela de laudo?")) {
      this.confirmacao.fileDCMId = this.fileDCM.Id;
      this.confirmacao.prioridade = this.fileDCM.prioridade;
      this.confirmacao.statusExames = "laudar";

      this.exameService.Confirmacao(this.confirmacao).subscribe(
        msg => {
          if (!msg.erro) {
            this.DivError("Laudo de Exame salvo com sucesso!");
            console.log("Laudo de Exame salvo com sucesso!");
            this.fileDCM.statusExamesFormatado = msg.id; //id retorna o novo status do exame
            window.close();
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          this.DivError("Não foi possível Salvar o laudo!");
          console.log("Não foi possível Salvar o laudo!");
        }
      );
    }
  }

  Assinar() {
    let validaTipoEstudo = this.listatipoEstudo.filter(x => x.status == true);
    if (validaTipoEstudo.length > 0) {
      this.VerificaRevisao();
    } else {
      this.SelecionarTipoEstudo(true);
    }
  }

  SalvarLaudo(subStatusExames: string = "novo") {
    if (this.confirmacao.historiaClinica && this.confirmacao.historiaClinica.trim()) {
      this.confirmacao.fileDCMId = this.fileDCM.Id;
      this.confirmacao.listaTipoEstudo = this.listatipoEstudo
        .filter(x => x.status == true)
        .map(({ Id }) => Id);
      this.confirmacao.prioridade = this.fileDCM.prioridade;
      this.confirmacao.statusExames = "laudado";
      this.confirmacao.subStatusExames = subStatusExames;

      this.exameService.Confirmacao(this.confirmacao).subscribe(
        msg => {
          if (!msg.erro) {
            console.log("Não foi possível Salvar o laudo!", msg);
            this.fileDCM.statusExamesFormatado = msg.id; //id retorna o novo status do exame
            window.close();
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          this.DivError("Não foi possível Salvar o laudo!");
          console.log("Não foi possível Salvar o laudo!");
        }
      );
    } else {
      this.DivError("A descrição do laudo é obrigatória!");
      console.log("A descrição do laudo é obrigatória!");
    }
  }

  Revisao() {
    const activeModal = this.modalService.open(RevisorComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout",
      windowClass: "modalxxl"
    });
    activeModal.componentInstance.fileDCM = this.fileDCM;
    activeModal.componentInstance.passEntry.subscribe((receivedEntry) => {
      if (receivedEntry) {
        this.SalvarLaudo();
      } else {
        this.SalvarLaudo("laudar_segundaLeitura");
      }
    });
  }

  VerificaRevisao() {
    this.exameService.VerificaRevisao(this.fileDCM.Id).subscribe(
      revisao => {
        if(revisao){
          this.Revisao();
        }else{
          this.SalvarLaudo();
        }
      },
      erro => {
        this.exameService.erro(erro);
        this.DivError("Erro ao verificar a revisão!");
        console.log("Erro ao verificar a revisão!!");
      }
    );
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.DivError(erro);
        console.log(erro);
      });
    }
  }

}
