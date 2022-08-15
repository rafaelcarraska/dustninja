
import { HttpClient } from "@angular/common/http";
import { Component, Input, EventEmitter, Output } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { ComboboxComponent } from "../../../../@core/model/combobox/combobox.component";


@Component({
  moduleId: module.id,
  selector: "./tipoEstudo",
  templateUrl: "./tipoEstudo.component.html",
  styleUrls: [
    "../../../../@theme/styles/modal.scss",
    "./tipoEstudo.component.scss"
  ]
})
export class TipoEstudoComponent {
  @Output() passEntry: EventEmitter<any> = new EventEmitter();
  @Input()
  modalHeader: string = "Tipo de Estudo";
  listatipoEstudo: ComboboxComponent[];
  botaoAssinar: boolean;
  count: boolean;

  constructor(
    private activeModal: NgbActiveModal,
    private http: HttpClient
  ) {
  }

  ngOnInit() {
    this.AtualizaBotoes();
  }

  changeValue(){
    this.AtualizaBotoes();
  }

  Cancelar(){
    this.listatipoEstudo.forEach(x => {
      x.status = false;
    });
    this.closeModal();
  }

  Selecionar(){
    this.closeModal();
  }

  Assinar(){
    this.passEntry.emit(true);
    this.closeModal();
  }

  closeModal() {
    this.activeModal.close();
  }

  AtualizaBotoes(){
    this.count = this.listatipoEstudo.filter(x =>
      x.status == true
    ).length > 0;
  }
}
