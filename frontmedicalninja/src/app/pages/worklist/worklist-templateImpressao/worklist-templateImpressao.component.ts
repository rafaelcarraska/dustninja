import { TemplateImpressaoService } from '../../../@core/data/templateImpressao.service';
import { ExameService } from '../../../@core/data/exame.service';
import { FileDCMComponent } from '../../../@core/model/fileDCM/fileDCM.component';
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { ComboboxComponent } from '../../../@core/model/combobox/combobox.component';

@Component({
  moduleId: module.id,
  selector: "./worklist-templateImpressao",
  templateUrl: "./worklist-templateImpressao.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./worklist-templateImpressao.component.scss"
  ]
})
export class WorklistTemplateImpressaoComponent {
  @Input()
  modalHeader: string = "";
  fileDCM: FileDCMComponent = new FileDCMComponent();
  templateImpressaoid: string;
  listaTemplateImpressao: ComboboxComponent[];
  exameService: ExameService;
  templateImpressaoService: TemplateImpressaoService;

  constructor(
    private activeModal: NgbActiveModal,
    exameService: ExameService,
    templateImpressaoService: TemplateImpressaoService,
  ) {
    this.exameService = exameService;
    this.templateImpressaoService = templateImpressaoService;
  }

  ngOnInit() {
    this.TemplateImpresasaoUtilizado(this.fileDCM.Id);
  }

  closeModal() {
    this.activeModal.close();
  }

  onChange(value: string) {
    this.templateImpressaoid = value;
  }

  GetLaudoPage(){
    if(this.templateImpressaoid && this.fileDCM.Id){
      window.open(`./#/laudopdf/${this.fileDCM.Id}_${this.templateImpressaoid}`, "laudopdf", 'width=1200,height=900');
      this.closeModal();
    }else{
      console.log("Não foi possível gerar laudo!");
      this.templateImpressaoService.showToast(
        "error",
        "Não foi possível gerar laudo!"
      );
    }
  }

  LoadTemplateImpressao() {
    this.templateImpressaoService.ListaComboByFacility(this.fileDCM.facilityId).subscribe(
      listaTemplateImpressao => {
        if (!listaTemplateImpressao){
          console.log("Nenhum template de impressão localizado.");
        }else{
          this.listaTemplateImpressao = listaTemplateImpressao;
          if(this.listaTemplateImpressao.length == 1){
            this.GetLaudoPage();
          }
        }
      },
      erro => {
        this.templateImpressaoService.erro(erro);
      }
    );
  }

  TemplateImpresasaoUtilizado(fileDCMId: string) {
    this.templateImpressaoService.TemplateImpresasaoUtilizado(fileDCMId).subscribe(
      templateImpressaoid => {
        if (!templateImpressaoid){
           console.log("template de impressão não localizado.");
        }
        this.templateImpressaoid = templateImpressaoid.id;
        this.LoadTemplateImpressao();
      },
      erro => {
        this.templateImpressaoService.erro(erro);
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
