
import { HttpClient } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { ComboboxComponent } from "../../../../@core/model/combobox/combobox.component";
import { NotaHistoricoComponent } from "../../../../@core/model/NotaHistorico/notaHistorico.component";


@Component({
  moduleId: module.id,
  selector: "./historicoClinico",
  templateUrl: "./historicoClinico.component.html",
  styleUrls: [
    "../../../../@theme/styles/modal.scss",
    "./historicoClinico.component.scss"
  ]
})
export class HistoricoClinicoComponent {
  @Input()
  modalHeader: string = "Histórico Clínico";
  historicoClinico: NotaHistoricoComponent = new NotaHistoricoComponent();

  constructor(
    private activeModal: NgbActiveModal,
    private http: HttpClient
  ) {

  }

  ngOnInit() {
  }

  closeModal() {
    this.activeModal.close();
  }


}
