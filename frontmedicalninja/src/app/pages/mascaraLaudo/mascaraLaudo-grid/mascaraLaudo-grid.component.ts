import { Component } from "@angular/core";
import { MascaraLaudoComponent } from "../mascaraLaudo.component";
import { MascaraLaudoService } from "../../../@core/data/mascaraLaudo.service";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import {ToasterConfig} from "angular2-toaster";

import "style-loader!angular2-toaster/toaster.css";
import { Router } from "@angular/router";
import { ConfiguracaoComponent } from "../../../@core/model/configuracao/configuracao.component";
import { ConfiguracaoService } from "../../../@core/data/configuracao.service";
import { PagerService } from "../../../@core/data/pager.service";
import { WorklistService } from "../../../@core/data/worklist.service";
import { TemplateImpressaoService } from "../../../@core/data/templateImpressao.service";

@Component({
  selector: "mascaraLaudo-grid",
  templateUrl: "./mascaraLaudo-grid.component.html",
  styleUrls: [
    "../../../@theme/styles/grid.scss",
    "./mascaraLaudo-grid.component.scss"
  ]
})
export class MascaraLaudoGridComponent {
  public searchString: string;
  listaMascara: MascaraLaudoComponent[] = [];
  configuracao: ConfiguracaoComponent;
  exibirDivConfiguracoes: boolean = false;
  load: boolean = false;
  worklistservice: WorklistService;
  pagerservice: PagerService;
  configuracaoservice: ConfiguracaoService;
  templateImpressaoService: TemplateImpressaoService;
  mascaraLaudoservice: MascaraLaudoService;

  constructor(
    mascaraLaudoservice: MascaraLaudoService,
    worklistservice: WorklistService,
    private readonly router: Router,
    pagerservice: PagerService,
    configuracaoservice: ConfiguracaoService,
    templateImpressaoService: TemplateImpressaoService,

  ) {
    this.worklistservice = worklistservice;
    this.mascaraLaudoservice = mascaraLaudoservice;
    this.LoadGrid();
    this.pagerservice = pagerservice;
    this.configuracaoservice = configuracaoservice;
    this.templateImpressaoService = templateImpressaoService;
    this.BuscarConfiguração();

  }
  config: ToasterConfig;

  pager: any = {};
  pagedItems: any[];

  ngAfterViewInit() {
  }

  LoadGrid() {
    this.load = true;
    this.mascaraLaudoservice.lista().subscribe(
      listaMascara => {
        if (!listaMascara)
          this.mascaraLaudoservice.showToast("warning", "Nenhuma Máscara de Laudo localizada.");

        this.listaMascara = listaMascara;
        this.setPage(1);
        this.setfiltro();
        // console.log(listaMascara);
      },
      erro =>  {
        this.mascaraLaudoservice.erro(erro);
      },
      () =>{
        this.load = false;
      }
    );
  }

  Novo() {
    this.router.navigate(["/pages/mascaraLaudo/0"]);
  }

  Modal(mascaraLaudo: MascaraLaudoComponent) {
    this.router.navigate(["/pages/mascaraLaudo/"+mascaraLaudo.Id]);
  }

  Deletar(mascaraLaudo): void {
    if (window.confirm("Deseja deletar essa Máscara de Laudo?")) {
      this.mascaraLaudoservice.remove(mascaraLaudo).subscribe(
        msg => {
          if (!msg.erro) {
            let novoMascaraLaudo = this.listaMascara.slice(0);
            let indice = novoMascaraLaudo.indexOf(mascaraLaudo);
            novoMascaraLaudo.splice(indice, 1);
            this.listaMascara = novoMascaraLaudo;
            this.mascaraLaudoservice.showToast("success", "Máscara Laudo deletada com sucesso.");
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.mascaraLaudoservice.showToast("warning", "Aceso negado.");
          }else{
            this.mascaraLaudoservice.showToast("error", "Erro ao deletar a máscara de laudo.");
          }
        }
      );
    }
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.mascaraLaudoservice.showToast("warning", erro);
      });
    }
  }

  setPage(page: number) {
    if (page < 1 || page > this.pager.totalPages) {
        return;
    }
    // pega o objeto pager do serviço
    this.configuracao.pageSize = 10;

    this.pager = this.pagerservice.getPager(this.listaMascara.length, page, this.configuracao.pageSize);

    // pega a página atual de itens
    this.pagedItems = this.listaMascara.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  setfiltro() {
    if (!this.searchString || !Array.isArray(this.listaMascara)) {
      this.setPage(1);
      return;
    }

    let filter: any = {
      descricao: this.searchString,
      modalidade: this.searchString,
      status: this.searchString,
      insertUsuarioId: this.searchString,
      insertDataFormatada: this.searchString,
    };

    let filterKeys = Object.keys(filter);

    this.pagedItems = this.listaMascara.filter(item => {
      return filterKeys.some(keyName => {
        return (
          new RegExp(filter[keyName], "gi").test(item[keyName]) || filter[keyName] == "");
      });
    });
    this.pager = this.pagerservice.getPager(this.pagedItems.length, 1, this.configuracao.pageSize);
    this.pagedItems = this.pagedItems.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  BuscarConfiguração(){
    this.configuracaoservice.lista().subscribe(
      configuracao => {
        if (!configuracao){
          this.configuracao.pageSize = 10;
        }else{
          this.configuracao = configuracao;
        }
      },
      erro => {
        this.configuracaoservice.erro(erro);
        this.configuracao.pageSize = 10;
      },
      () => {
        this.LoadGrid();
      }
    );
  }

}
