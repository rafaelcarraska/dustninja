import { NotaHistoricoComponent } from "./notaHistorico.component";
import { LogComponent } from "../log/logComponent";

export class NotaComponent{
  Id: string;
  fileDCMId: string;
  listaNota: NotaHistoricoComponent[];
  status: boolean;
  countNota: number;
  log: LogComponent = new LogComponent();
}
