import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { EmpresaService } from "../../../@core/data/empresa.service";
import { ToasterConfig } from "angular2-toaster";

import "./filtros-form.loader";
import "ckeditor";
import { FiltrosComponent, FiltroStatusComponent, FiltroGeraisComponent, FiltroDatasComponent, FiltroOrdemComponent, FiltroPerfilComponent } from "../filtros.component";
import { FiltrosService } from "../../../@core/data/filtros.service";
import { ComboboxComponent } from "../../../@core/model/combobox/combobox.component";
import { PerfilService } from "../../../@core/data/perfil.service";

@Component({
  moduleId: module.id,
  selector: "filtros",
  templateUrl: "./filtros-form.component.html",
  styleUrls: [
    "../../../@theme/styles/form.scss",
    "./filtros-form.component.scss"
  ]
})

export class FiltrosFormComponent implements OnInit {
  filtro: FiltrosComponent = new FiltrosComponent();
  filtrosservice: FiltrosService;
  perfilService: PerfilService;
  empresaservice: EmpresaService;
  errors: string[];
  erroDescricao: string;
  listaComboPerfil: ComboboxComponent[];
  listaComboStatus: ComboboxComponent[];
  listaComboDatas: ComboboxComponent[];
  listaComboGerais: ComboboxComponent[];
  listaComboOrdem: ComboboxComponent[];
  tabIndex: string = 'Perfil';

  filtroStatusSelecionado: FiltroStatusComponent = new FiltroStatusComponent();
  filtroGeraisSelecionado: FiltroGeraisComponent = new FiltroGeraisComponent();
  filtroDatasSelecionado: FiltroDatasComponent = new FiltroDatasComponent();
  filtroOrdemSelecionado: FiltroOrdemComponent = new FiltroOrdemComponent();
  filtroPerfilSelecionado: FiltroStatusComponent = new FiltroStatusComponent();

  constructor(
    private route: ActivatedRoute,
    private readonly router: Router,
    filtrosservice: FiltrosService,
    empresaservice: EmpresaService,
    perfilService: PerfilService
  ) {
    this.filtro.status = true;
    this.filtrosservice = filtrosservice;
    this.empresaservice = empresaservice;
    this.perfilService = perfilService;

    this.ListaComboStatus();
    this.ListaComboPerfil();
    this.ListaComboGerais();
    this.ListaComboDatas();
    this.ListaComboOrdem();
    this.route.params.subscribe(params => this.LoadFiltro(params.id));
  }
  config: ToasterConfig;

  ngOnInit(): void {

  }

  LoadFiltro(Id: string) {
    if (Id != "0") {
      this.filtrosservice.ListaFiltro(Id).subscribe(
        filtro => {
          if (!filtro)
            this.filtrosservice.showToast(
              "warning",
              "Nenhum filtro localizado."
            );

          this.filtro = filtro;
          // this.filtro.listaFiltroStatus = [];
          // this.filtro.listaFiltroDatas = [];
          // this.filtro.listaFiltroGerais = [];
          // this.filtro.listaFiltroOrdem = [];
        },
        erro => this.filtrosservice.showToast("error", erro)
      );
    }
  }

  ListaComboStatus() {
    this.filtrosservice.ListaComboStatus().subscribe(
      listaComboStatus => {
        if (!listaComboStatus) {
          this.filtrosservice.showToast(
            "warning",
            "Nenhum filtro status localizado."
          );
        } else {
          this.filtroStatusSelecionado.Id = listaComboStatus[0].id;
          this.filtroStatusSelecionado.descricao = listaComboStatus[0].descricao;
        }

        this.listaComboStatus = listaComboStatus;
      },
      erro => this.filtrosservice.showToast("error", erro)
    );
  }

  ListaComboPerfil() {
    this.perfilService.listaCombo().subscribe(
      listaComboPerfil => {
        if (!listaComboPerfil) {
          this.perfilService.showToast(
            "warning",
            "Nenhum perfil localizado."
          );
        } else {
          this.filtroPerfilSelecionado.Id = listaComboPerfil[0].id;
          this.filtroPerfilSelecionado.descricao = listaComboPerfil[0].descricao;
        }

        this.listaComboPerfil = listaComboPerfil;
      },
      erro => this.filtrosservice.showToast("error", erro)
    );
  }

  ListaComboDatas() {
    this.filtrosservice.ListaComboDatas().subscribe(
      listaComboDatas => {
        if (!listaComboDatas)
          this.filtrosservice.showToast(
            "warning",
            "Nenhum filtro datas localizado."
          );

        this.listaComboDatas = listaComboDatas;
      },
      erro => this.filtrosservice.showToast("error", erro)
    );
  }

  ListaComboGerais() {
    this.filtrosservice.ListaComboGerais().subscribe(
      listaComboGerais => {
        if (!listaComboGerais)
          this.filtrosservice.showToast(
            "warning",
            "Nenhum filtro datas localizado."
          );

        this.listaComboGerais = listaComboGerais;
      },
      erro => this.filtrosservice.showToast("error", erro)
    );
  }

  ListaComboOrdem() {
    this.filtrosservice.ListaComboOrdem().subscribe(
      listaComboOrdem => {
        if (!listaComboOrdem){
          this.filtrosservice.showToast(
            "warning",
            "Nenhum filtro datas localizado."
          );
        }else{
          this.filtroOrdemSelecionado.Id = listaComboOrdem[0].id;
          this.filtroOrdemSelecionado.descricao = listaComboOrdem[0].descricao;
          this.filtroOrdemSelecionado.ordem = listaComboOrdem[0].ordem;
        }

        this.listaComboOrdem = listaComboOrdem;
      },
      erro => this.filtrosservice.showToast("error", erro)
    );
  }

  AddStatus(filtroStatusSelecionado: FiltroStatusComponent) {
    if (this.filtro.listaFiltroStatus != undefined) {
      if (this.filtro.listaFiltroStatus.filter(f => f.Id == filtroStatusSelecionado.Id).length > 0) {
        this.filtrosservice.showToast(
          "warning",
          "Esse Status já foi adicionado."
        );
        return;
      }
    } else {
      this.filtro.listaFiltroStatus = [];
    }
    let filtroStatus = this.listaComboStatus.filter(f => f.Id == filtroStatusSelecionado.Id)[0];

    let filtroStatusAdd = new FiltroStatusComponent();
    filtroStatusAdd.descricao = filtroStatus.descricao;
    filtroStatusAdd.Id = filtroStatus.Id;

    this.filtro.listaFiltroStatus.push(filtroStatusAdd);

  }

  AddPerfil(filtroPerfilSelecionado: FiltroPerfilComponent) {
    if (this.filtro.listaPerfil != undefined) {
      if (this.filtro.listaPerfil.filter(f => f.Id == filtroPerfilSelecionado.Id).length > 0) {
        this.filtrosservice.showToast(
          "warning",
          "Esse perfil já foi adicionado."
        );
        return;
      }
    } else {
      this.filtro.listaPerfil = [];
    }
    let filtroPerfil = this.listaComboPerfil.filter(f => f.Id == filtroPerfilSelecionado.Id)[0];

    let filtroPerfilAdd = new FiltroPerfilComponent();
    filtroPerfilAdd.descricao = filtroPerfil.descricao;
    filtroPerfilAdd.Id = filtroPerfil.Id;

    this.filtro.listaPerfil.push(filtroPerfilAdd);

  }

  AddDatas() {

  }

  AddGerais() {

  }

  AddOrdem(filtroOrdemSelecionado: FiltroOrdemComponent, ordem: string) {
    if (this.filtro.listaFiltroOrdem != undefined) {
      if (this.filtro.listaFiltroOrdem.filter(f => f.Id == filtroOrdemSelecionado.Id).length > 0) {
        this.filtrosservice.showToast(
          "warning",
          "Essa Ordem já foi adicionado."
        );
        return;
      }
    } else {
      this.filtro.listaFiltroOrdem = [];
    }
    let filtroOrdem = this.listaComboOrdem.filter(f => f.Id == filtroOrdemSelecionado.Id)[0];

    let filtroOrdemAdd = new FiltroOrdemComponent();
    filtroOrdemAdd.descricao = filtroOrdem.descricao;
    filtroOrdemAdd.Id = filtroOrdem.Id;
    filtroOrdemAdd.ordem = ordem;
    filtroOrdemAdd.prioridade = this.filtro.listaFiltroOrdem.length;

    this.filtro.listaFiltroOrdem.push(filtroOrdemAdd);
  }

  DeletarFiltroStatus(FiltroStatus: FiltroStatusComponent) {
    let novoFiltroStatus = this.filtro.listaFiltroStatus.slice(0);
    let indice = novoFiltroStatus.indexOf(FiltroStatus);
    novoFiltroStatus.splice(indice, 1);
    this.filtro.listaFiltroStatus = novoFiltroStatus;
  }

  DeletarFiltroPerfil(filtroPerfil: FiltroPerfilComponent) {
    let novoFiltroPerfil = this.filtro.listaPerfil.slice(0);
    let indice = novoFiltroPerfil.indexOf(filtroPerfil);
    novoFiltroPerfil.splice(indice, 1);
    this.filtro.listaPerfil = novoFiltroPerfil;
  }

  DeletarFiltroOrdem(filtroOrdem: FiltroOrdemComponent) {
    let novoFiltroOrdem = this.filtro.listaFiltroOrdem.slice(0);
    let indice = novoFiltroOrdem.indexOf(filtroOrdem);
    novoFiltroOrdem.splice(indice, 1);
    this.filtro.listaFiltroOrdem = novoFiltroOrdem;
  }

  UpFiltroOrdem(filtroOrdem: FiltroOrdemComponent){

  }

  DownFiltroOrdem(filtroOrdem: FiltroOrdemComponent){

  }

  ParticularChange() {
    if (this.filtro.particular && this.tabIndex == "Perfil") {
      this.tabIndex = "Filtro Status";
    }
  }

  OnTabChange(ev: any) {
    if (this.filtro.particular) {
      this.tabIndex = "Filtro Status";
    } else {
      this.tabIndex = ev.tabTitle;
    }
  }

  Salvar() {
    this.LimparErros();
    this.filtrosservice.salva(this.filtro).subscribe(
      msg => {
        if (!msg.erro) {
          if (!this.filtro.Id && msg.id) {
            this.filtro.Id = msg.id;
          }
          this.filtrosservice.showToast(
            "success",
            "Filtro salvo com sucesso!"
          );
          this.router.navigate(["/pages/filtros/filtros-grid"]);
        } else {
          this.ShowErrors(msg.erro);
        }
      },
      erro => {
        console.log(erro);
        if (erro.status === 400) {
          this.InputErros(erro.error);
        }
        if (erro.status === 401 || erro.status === 403) {
          this.filtrosservice.showToast("warning", "Aceso negado.");
        }
        this.filtrosservice.showToast(
          "error",
          "Não foi possível salvar o filtro!"
        );
      }
    );
  }

  Cancelar() {
    this.router.navigate(["/pages/filtros/filtros-grid"]);
  }

  LimparErros() {
    this.errors = [];
    this.erroDescricao = "";
  }

  InputErros(validationErrorDictionary) {
    for (var fieldName in validationErrorDictionary) {
      if (validationErrorDictionary.hasOwnProperty(fieldName)) {
        switch (fieldName) {
          case "descricao":
            this.erroDescricao = validationErrorDictionary[fieldName];
            break;
          default:
            this.filtrosservice.showToast("error", validationErrorDictionary[fieldName]);
            break;
        }
      }
    }
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      console.log(erro);
      this.filtrosservice.showToast(
        "error",
        erro
      );
    });
  }

}
