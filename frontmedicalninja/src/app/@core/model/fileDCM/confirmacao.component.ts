import { DesmembrarComponent } from "../desmembrar/desmembrarComponenr";

export class ConfirmacaoComponent{
  fileDCMId: string;
  prioridade: string;
  statusExames: string;
  subStatusExames: string;
  templateImpressaoid: string;
  historiaClinica: string;
  listaTipoEstudo: string[];
  desmembrar: DesmembrarComponent = new DesmembrarComponent();
}
