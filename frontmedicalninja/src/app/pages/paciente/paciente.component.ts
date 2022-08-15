import { Component } from '@angular/core';
import { LogComponent } from '../../@core/model/log/logComponent';

@Component({
  moduleId: module.id,
  selector: 'paciente',
  template: `<router-outlet></router-outlet>`,
})
export class PacienteComponent {
  Id: string;
  pkPostgre: number;
  dataNascimento: Date;
  nome: string;
  namePrefix: string;
  middleName: string;
  giveName: string;
  pacienteIdDCM: string;
  sexo: string;
  nomeCompleto: string;
  dataNascimento_formatada: string;
  aeTitle: string;
  facility: string;
  idade: string;
  countExames: number;
  status: boolean;
  log: LogComponent = new LogComponent();
}
