import { FileDCMComponent } from '../../../@core/model/fileDCM/fileDCM.component';
import { HttpClient } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { PacienteService } from '../../../@core/data/paciente.service';
import { PagerService } from '../../../@core/data/pager.service';
import { PacienteComponent } from '../../paciente/paciente.component';
import { ToasterConfig } from 'angular2-toaster';
import { TrocaPacienteComponent } from '../../../@core/model/fileDCM/trocaPaciente.component';

@Component({
  moduleId: module.id,
  selector: "./exame-paciente",
  templateUrl: "./exame-paciente.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "../../../@theme/styles/grid.scss",
    "./exame-paciente.component.scss"
  ]
})
export class ExamePacienteComponent {
  @Input()
  modalHeader: string = "";
  public searchString: string;
  fileDCM: FileDCMComponent = new FileDCMComponent();
  pacienteservice: PacienteService;
  pagerservice: PagerService;
  listaPaciente: PacienteComponent[] = [];
  load: boolean = true;
  newTrocaPaciente: TrocaPacienteComponent = new TrocaPacienteComponent();

  constructor(
    private activeModal: NgbActiveModal,
    private http: HttpClient,
    pacienteservice: PacienteService,
    pagerservice: PagerService,
  ) {
    this.pacienteservice = pacienteservice;
    this.pagerservice = pagerservice;
  }
  config: ToasterConfig;
  pager: any = {};
  pagedItems: any[];

  ngOnInit() {
    this.LoadGridPacientes();
  }

  closeModal() {
    this.activeModal.close();
  }

  TrocaPaciente(paciente: PacienteComponent){
    this.load = true;
    this.newTrocaPaciente.pacienteId = paciente.Id;
    this.newTrocaPaciente.fileDCMId = this.fileDCM.Id;

    this.pacienteservice.TrocaPaciente(this.newTrocaPaciente).subscribe(
      msg => {
        if (!msg.erro) {
          this.pacienteservice.showToast(
            "success",
            "Paciente alterado com sucesso!"
          );
        } else {
          this.ShowErrors(msg.erro);
        }
        this.closeModal();
      },
      erro => {
        console.log(erro); //remover
        if (erro.status === 400) {
          this.pacienteservice.showToast(
            "error",
            erro.error
          );
        }
        if (erro.status === 401 || erro.status === 403) {
          this.pacienteservice.showToast("warning", "Aceso negado.");
        }
        this.pacienteservice.showToast(
          "error",
          "Não foi possível salvar o paciente!"
        );
      },
      () => {
        this.load = false;
      }
    );
  }

  LoadGridPacientes() {
    this.pacienteservice.ListaFacility(this.fileDCM.facilityId).subscribe(
      listaPaciente => {
        if (!listaPaciente)
          this.pacienteservice.showToast("warning", "Nenhum paciente localizada.");

        this.listaPaciente = listaPaciente;
        this.setPage(1);
        this.setfiltro();
        // console.log(listaPaciente);
      },
      erro => {
        this.pacienteservice.erro(erro);
      },
      () => {
        this.load = false;
      }
    );
  }

  setfiltro() {
    if (!this.searchString || !Array.isArray(this.listaPaciente)) {
      this.setPage(1);
      return;
    }

    let filter: any = {
      nomeCompleto: this.searchString,
      dataNascimento_formatada: this.searchString,
      pacienteIdDCM: this.searchString
    };

    let filterKeys = Object.keys(filter);

    this.pagedItems = this.listaPaciente.filter(item => {
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
    this.pager = this.pagerservice.getPager(this.listaPaciente.length, page, 8);

    // pega a página atual de itens
    this.pagedItems = this.listaPaciente.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      console.log(erro); //toaster
      this.pacienteservice.showToast(
        "error",
        erro
      );
    });
  }

}
