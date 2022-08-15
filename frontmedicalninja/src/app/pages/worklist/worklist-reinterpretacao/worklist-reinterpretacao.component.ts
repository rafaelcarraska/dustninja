import { TemplateImpressaoService } from '../../../@core/data/templateImpressao.service';
import { ExameService } from '../../../@core/data/exame.service';
import { FileDCMComponent } from '../../../@core/model/fileDCM/fileDCM.component';
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { ConfirmacaoComponent } from '../../../@core/model/fileDCM/confirmacao.component';
import { ComboboxComponent } from '../../../@core/model/combobox/combobox.component';

@Component({
  moduleId: module.id,
  selector: "./worklist-log",
  templateUrl: "./worklist-reinterpretacao.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./worklist-reinterpretacao.component.scss"
  ]
})
export class WorklistReiterpretacaoComponent {
  @Input()
  modalHeader: string = "";
  fileDCM: FileDCMComponent = new FileDCMComponent();
  confirmacao: ConfirmacaoComponent = new ConfirmacaoComponent();
  recent: any[];
  listaTemplateImpressao: ComboboxComponent[];
  usuarioId: string = environment.usuarioId;
  exameService: ExameService;
  templateImpressaoService: TemplateImpressaoService;
  reinterpretacao: boolean = false;

  constructor(
    private activeModal: NgbActiveModal,
    exameService: ExameService,
    private http: HttpClient
  ) {
    this.exameService = exameService;
  }

  ngOnInit() {
  }

  closeModal() {
    this.activeModal.close();
  }

  Confirmacao(statusExames: string){
    // console.log('statusExames', statusExames);
    this.confirmacao.fileDCMId = this.fileDCM.Id;
    this.confirmacao.prioridade = this.fileDCM.prioridade;
    this.confirmacao.statusExames = statusExames;
    this.confirmacao.subStatusExames = "laudar_reiterpretacao";
    this.SalvarConfirmacao();
  }

  onChange(value: string) {
    this.confirmacao.templateImpressaoid = value;
  }

  ConfirmacaoReinterpretacao(){
    this.reinterpretacao = true;
  }

  SalvarConfirmacao(){
    this.exameService.Confirmacao(this.confirmacao).subscribe(
      msg => {
        if (!msg.erro) {
          this.exameService.showToast(
            "success",
            "Reinterpretação do exame realizada com sucesso!"
          );
          this.fileDCM.statusExamesFormatado = msg.id; //id retorna o novo status do exame
          this.fileDCM.subStatusExamesFormatado = "laudar_reiterpretacao";
          this.closeModal();
        } else {
          this.ShowErrors(msg.erro);
        }
      },
      erro => {
        this.exameService.showToast(
          "error",
          "Não foi possível realizar a Reinterpretação!"
        );
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
