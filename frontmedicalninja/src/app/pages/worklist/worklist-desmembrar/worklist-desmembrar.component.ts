import { ExameService } from './../../../@core/data/exame.service';
import { HttpClient } from "@angular/common/http";
import { Component, Input, Output, EventEmitter } from "@angular/core";
import { NgbActiveModal, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FileDCMComponent } from '../../../@core/model/fileDCM/fileDCM.component';
import { DesmembrarComponent } from '../../../@core/model/desmembrar/desmembrarComponenr';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  moduleId: module.id,
  selector: "./worklist-desmembrar",
  templateUrl: "./worklist-desmembrar.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./worklist-desmembrar.component.scss"
  ]
})
export class WorklistDesmembrarComponent {
  @Output() passEntry: EventEmitter<any> = new EventEmitter();
  @Input()
  modalHeader: string = "";
  fileDCM: FileDCMComponent = new FileDCMComponent();
  confirmacao: boolean = false;
  desmembrar: DesmembrarComponent = new DesmembrarComponent();
  exameService: ExameService;

  errors: string[];
  erroNovaDescricao: string;

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

  ngOnInit() {
    this.desmembrar.fileDCMId = this.fileDCM.Id;
    this.desmembrar.novaDescricao = this.fileDCM.studyDesc;
    this.desmembrar.novosExames = [];
    this.desmembrar.novosExames.push('');

    // console.log('desmembrar', this.desmembrar);
  }

  closeModal() {
    this.activeModal.close();
  }

  Desmembrar() {
    this.LimparErros();
    this.desmembrar.novosExames = this.desmembrar.novosExames.filter(Boolean);
    if(this.desmembrar.novosExames.length == 0){
      this.desmembrar = new DesmembrarComponent();
    }
    this.desmembrar.confirmacao = this.confirmacao;
    if (this.confirmacao) {
      this.passEntry.emit(this.desmembrar);
      this.closeModal();
    } else {
      this.exameService.Desmembrar(this.desmembrar).subscribe(
        msg => {
          if (!msg.erro) {
            this.exameService.showToast(
              "success",
              "Exame desmembrado com sucesso!"
            );
            this.router.navigate([`/pages/worklist/paciente:${this.fileDCM.paciente.Id}`]);
            this.closeModal();
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 400) {
            this.InputErros(erro.error);
          }
          if (erro.status === 401 || erro.status === 403) {
            this.exameService.showToast("warning", "Aceso negado.");
          }
          this.exameService.showToast(
            "error",
            "Não foi possível desmembrar o exame!"
          );
        }
      );
    }
  }

  LimparErros() {
    this.errors = [];
    this.erroNovaDescricao = "";
  }

  InputErros(validationErrorDictionary) {
    for (var fieldName in validationErrorDictionary) {
      if (validationErrorDictionary.hasOwnProperty(fieldName)) {
        switch (fieldName) {
          case "NovaDescricao":
            this.erroNovaDescricao = validationErrorDictionary[fieldName];
            break;
          default:
            this.exameService.showToast("error", validationErrorDictionary[fieldName]);
            break;
        }
      }
    }
  }

  AddExame() {
    this.desmembrar.novosExames.push('');
  }

  RemoverExame(indice: number) {
    this.desmembrar.novosExames.splice(indice, 1);
  }

  trackByFn(index: any, item: any) {
    return index;
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.exameService.showToast("warning", erro);
      });
    }
  }
}
