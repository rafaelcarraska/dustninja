import { Component } from "@angular/core";
import { PacienteComponent } from "../paciente.component";
import { PacienteService } from "../../../@core/data/paciente.service";
import { ToasterConfig } from "angular2-toaster";
import "style-loader!angular2-toaster/toaster.css";
import { Router } from "@angular/router";
import { ConfiguracaoComponent } from "../../../@core/model/configuracao/configuracao.component";
import { PagerService } from "../../../@core/data/pager.service";
import { ConfiguracaoService } from "../../../@core/data/configuracao.service";

@Component({
  selector: "paciente-grid",
  templateUrl: "./paciente-grid.component.html",
  styleUrls: [
    "../../../@theme/styles/grid.scss",
    "./paciente-grid.component.scss"
  ]
})
export class PacienteGridComponent {
  public searchString: string;
  listaPaciente: PacienteComponent[] = [];
  configuracao: ConfiguracaoComponent;
  pacienteservice: PacienteService;
  configuracaoservice: ConfiguracaoService;
  pagerservice: PagerService;

  constructor(
    pacienteservice: PacienteService,
    private readonly router: Router,
    pagerservice: PagerService,
    configuracaoservice: ConfiguracaoService,
  ) {
    this.pacienteservice = pacienteservice;
    this.pagerservice = pagerservice;
    this.configuracaoservice = configuracaoservice;
    this.LoadGrid();
    this.BuscarConfiguração();

  }
  config: ToasterConfig;

  pager: any = {};
  pagedItems: any[];

  LoadGrid() {
    this.pacienteservice.lista().subscribe(
      listaPaciente => {
        if (!listaPaciente)
          this.pacienteservice.showToast("warning", "Nenhum paciente localizada.");

        this.listaPaciente = listaPaciente;
        this.setPage(1);
        this.setfiltro();
        // console.log(listaPaciente);
      },
      erro =>  {
        this.pacienteservice.erro(erro);
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
      facility: this.searchString
    };

    let filterKeys = Object.keys(filter);

    this.pagedItems = this.listaPaciente.filter(item => {
      return filterKeys.some(keyName => {
        return (
          new RegExp(filter[keyName], "gi").test(item[keyName]) || filter[keyName] == "");
      });
    });
    this.pager = this.pagerservice.getPager(this.pagedItems.length, 1, this.configuracao.pageSize);
    this.pagedItems = this.pagedItems.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  setPage(page: number) {
    if (page < 1 || page > this.pager.totalPages) {
        return;
    }
    // pega o objeto pager do serviço
    this.configuracao.pageSize = 10;

    this.pager = this.pagerservice.getPager(this.listaPaciente.length, page, this.configuracao.pageSize);

    // pega a página atual de itens
    this.pagedItems = this.listaPaciente.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  Modal(paciente: PacienteComponent) {
    this.router.navigate(["/pages/paciente/"+paciente.Id]);
  }

  Deletar(paciente): void {
    if (window.confirm("Deseja deletar esse paciente?")) {
      this.pacienteservice.remove(paciente).subscribe(
        msg => {
          if (!msg.erro) {
            let novoPaciente = this.listaPaciente.slice(0);
            let indice = novoPaciente.indexOf(paciente);
            novoPaciente.splice(indice, 1);
            this.listaPaciente = novoPaciente;
            this.pacienteservice.showToast("success", "Paciente deletado com sucesso.");
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          console.log(erro);
          if (erro.status === 401 || erro.status === 403) {
            this.pacienteservice.showToast("warning", "Aceso negado.");
          }else{
            this.pacienteservice.showToast("error", "Erro ao deletar o paciente.");
          }
        }
      );
    }
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.pacienteservice.showToast("warning", erro);
      });
    }
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
