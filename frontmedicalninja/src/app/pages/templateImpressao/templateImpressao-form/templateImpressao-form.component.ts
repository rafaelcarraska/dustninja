import { TemplateImpressaoService } from "./../../../@core/data/templateImpressao.service";
import { TemplateImpressaoComponent } from "./../templateImpressao.component";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { EmpresaService } from "../../../@core/data/empresa.service";
import { ToasterConfig } from "angular2-toaster";
import "./templateImpressao-form.loader";
import "ckeditor";
import { TemplateAnexosComponent } from "../templateImpressao-anexos/templateImpressao-anexos.component";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
  moduleId: module.id,
  selector: "templateImpressao",
  templateUrl: "./templateImpressao-form.component.html",
  styleUrls: [
    "../../../@theme/styles/form.scss",
    "./templateImpressao-form.component.scss"
  ]
})
export class TemplateImpressaoFormComponent implements OnInit {

  templateImpressao: TemplateImpressaoComponent = new TemplateImpressaoComponent();
  listaTemplate: TemplateImpressaoComponent[] = [];
  templateImpressaoservice: TemplateImpressaoService;
  empresaservice: EmpresaService;
  ckeCabecalhoConfig: any;
  ckeCorpoConfig: any;
  ckeRodapeConfig: any;
  placeholdersCabecalho = ['Nome', 'ID', 'Sexo', 'Data_Nasc', 'Idade', 'Data_Hora', 'Data', 'Hora', 'Modalidade', 'Lateralidade', 'Descricao', 'Body_Part', 'DICOM_Institution', 'Data_Hora_Confirmação', 'Data_Hora_Recebimento', 'Nome_Unidade', 'RazaoSocial_Unidade', 'Nome_Empresa', 'RazaoSocial_Empresa', 'Laudo', 'LINHA1_MEDICO1', 'LINHA2_MEDICO1', 'LINHA3_MEDICO1', 'LINHA1_MEDICO2', 'LINHA2_MEDICO2', 'LINHA3_MEDICO2', 'DATA_ASSINATURA', 'IMAGEM_ASSINATURA1', 'IMAGEM_ASSINATURA2'];
  placeholdersCorpo = ['Nome', 'ID', 'Sexo', 'Data_Nasc', 'Idade', 'Data_Hora', 'Data', 'Hora', 'Modalidade', 'Lateralidade', 'Descricao', 'Body_Part', 'DICOM_Institution', 'Data_Hora_Confirmação', 'Data_Hora_Recebimento', 'Nome_Unidade', 'RazaoSocial_Unidade', 'Nome_Empresa', 'RazaoSocial_Empresa', 'Laudo', 'LINHA1_MEDICO1', 'LINHA2_MEDICO1', 'LINHA3_MEDICO1', 'LINHA1_MEDICO2', 'LINHA2_MEDICO2', 'LINHA3_MEDICO2', 'DATA_ASSINATURA', 'IMAGEM_ASSINATURA1', 'IMAGEM_ASSINATURA2'];
  placeholdersRodape = ['Nome', 'ID', 'Sexo', 'Data_Nasc', 'Idade', 'Data_Hora', 'Data', 'Hora', 'Modalidade', 'Lateralidade', 'Descricao', 'Body_Part', 'DICOM_Institution', 'Data_Hora_Confirmação', 'Data_Hora_Recebimento', 'Nome_Unidade', 'RazaoSocial_Unidade', 'Nome_Empresa', 'RazaoSocial_Empresa', 'Laudo', 'LINHA1_MEDICO1', 'LINHA2_MEDICO1', 'LINHA3_MEDICO1', 'LINHA1_MEDICO2', 'LINHA2_MEDICO2', 'LINHA3_MEDICO2', 'DATA_ASSINATURA', 'IMAGEM_ASSINATURA1', 'IMAGEM_ASSINATURA2'];
  extraPlugins = 'autocomplete,textmatch,toolbar,basicstyles,link,undo,divarea';
  placeholderFormat = '[[%placeholder%]]<span>&nbsp;</span>';
  errors: string[];
  erroDescricao: string;
  corpo: boolean = false;
  cabecalho: boolean = true;
  rodape: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private readonly router: Router,
    templateImpressaoservice: TemplateImpressaoService,
    empresaservice: EmpresaService,
    private modalService: NgbModal,
  ) {
    this.templateImpressao.status = true;
    this.templateImpressaoservice = templateImpressaoservice;
    this.empresaservice = empresaservice;

    this.route.params.subscribe(params => this.LoadTemplate(params.id));
  }
  config: ToasterConfig;

  ngOnInit(): void {
    this.ckeCabecalhoConfig = {
      allowedContent: false,
      extraPlugins: this.extraPlugins,
      forcePasteAsPlainText: true,
      editorConfig: 'pt-br',
    };
    this.ckeCabecalhoConfig.placeholder_select = {
      placeholders: this.placeholdersCabecalho,
      format: this.placeholderFormat
    };
    this.ckeCorpoConfig = {
      allowedContent: false,
      extraPlugins: this.extraPlugins,
      forcePasteAsPlainText: true,
      editorConfig: 'pt-br',
    };
    this.ckeCorpoConfig.placeholder_select = {
      placeholders: this.placeholdersCorpo,
      format: this.placeholderFormat
    };
    this.ckeRodapeConfig = {
      allowedContent: false,
      extraPlugins: this.extraPlugins,
      forcePasteAsPlainText: true,
      editorConfig: 'pt-br',
    };
    this.ckeRodapeConfig.placeholder_select = {
      placeholders: this.placeholdersRodape,
      format: this.placeholderFormat
    };
  }

  Anexos(templateImpressao: TemplateImpressaoComponent){
    const activeModal = this.modalService.open(TemplateAnexosComponent, {
      size: "lg",
      backdrop: "static",
      container: "nb-layout"
    });
    activeModal.componentInstance.templateImpressao = templateImpressao;
  }

  LoadTemplate(Id: string) {
    if (Id != "0") {
      this.templateImpressaoservice.ListaTemplateImpressao(Id).subscribe(
        templateImpressao => {
          if (!templateImpressao)
            this.templateImpressaoservice.showToast(
              "warning",
              "Nenhum template de impressão localizado."
            );
          this.templateImpressao = templateImpressao;
        },
        erro => this.templateImpressaoservice.showToast("error", erro)
      );
    }
  }

  Cancelar() {
    this.router.navigate(["/pages/templateImpressoes/templateImpressao-grid"]);
  }

  Salvar() {
    this.LimparErros();
    this.templateImpressaoservice.salva(this.templateImpressao).subscribe(
      msg => {
        if (!msg.erro) {
          if (!this.templateImpressao.Id && msg.id) {
            this.templateImpressao.Id = msg.id;
            this.listaTemplate.push(this.templateImpressao);
          }
          this.templateImpressaoservice.showToast(
            "success",
            "Template de impressão salvo com sucesso!"
          );
          this.router.navigate(["/pages/templateImpressoes/templateImpressao-grid"]);
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
          this.templateImpressaoservice.showToast("warning", "Aceso negado.");
        }
        this.templateImpressaoservice.showToast(
          "error",
          "Não foi possível salvar o template de impressaão!"
        );
      }
    );
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
          this.templateImpressaoservice.showToast("error", validationErrorDictionary[fieldName]);
            break;
        }
      }
    }
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      console.log(erro);
      this.templateImpressaoservice.showToast(
        "error",
        erro
      );
    });
  }

  mudaTab(tabs){
    switch(tabs.tabTitle) {
      case 'Cabeçalho': {
         this.cabecalho = true;
         this.corpo = false;
         this.rodape = false;
         break;
      }
      case 'Corpo': {
        this.cabecalho = false;
        this.corpo = true;
        this.rodape = false;
         break;
      }
      case 'Rodapé': {
        this.cabecalho = false;
        this.corpo = false;
        this.rodape = true;
        break;
     }
      default: {
        this.cabecalho = true;
        this.corpo = false;
        this.rodape = false;
         break;
      }
   }
  }
}
