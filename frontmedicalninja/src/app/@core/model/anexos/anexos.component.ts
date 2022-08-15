import { LogComponent } from "../log/logComponent";

export class AnexoComponent{
  Id: string;
  descricao: string;
  tamanho: string;
  arquivo: string;
  extensao: string;
  log: LogComponent = new LogComponent();
}
