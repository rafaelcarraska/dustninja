import { LogComponent } from "../log/logComponent";

export class ConfiguracaoComponent{
  Id: string;
  usuarioId: string;
  pageSize: number;
  status: boolean;
  log: LogComponent = new LogComponent();
  filtroPadrao: string;
}
