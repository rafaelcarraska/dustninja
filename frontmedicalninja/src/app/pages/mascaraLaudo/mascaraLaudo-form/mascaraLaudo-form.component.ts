import { MascaraLaudoService } from "./../../../@core/data/mascaraLaudo.service";
import { MascaraLaudoComponent } from "./../mascaraLaudo.component";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { EmpresaService } from "../../../@core/data/empresa.service";
import { ToasterConfig } from "angular2-toaster";

import "./mascaraLaudo-form.loader";
import "ckeditor";

@Component({
  moduleId: module.id,
  selector: "mascaraLaudo",
  templateUrl: "./mascaraLaudo-form.component.html",
  styleUrls: [
    "../../../@theme/styles/form.scss",
    "./mascaraLaudo-form.component.scss"
  ]
})
export class MascaraLaudoFormComponent implements OnInit {
  // [x: string]: any;
  mascaraLaudo: MascaraLaudoComponent = new MascaraLaudoComponent();
  listaMascara: MascaraLaudoComponent[] = [];
  mascaraLaudoservice: MascaraLaudoService;
  empresaservice: EmpresaService;
  ckeConfig: any;
  errors: string[];
  erroDescricao: string;
  erroModalidade: string;


  constructor(
    private route: ActivatedRoute,
    private readonly router: Router,
    mascaraLaudoservice: MascaraLaudoService,
    empresaservice: EmpresaService
  ) {
    this.mascaraLaudo.status = true;
    this.mascaraLaudoservice = mascaraLaudoservice;
    this.empresaservice = empresaservice;

    this.route.params.subscribe(params => this.LoadMascara(params.id));
  }
  config: ToasterConfig;

  ngOnInit() {
    this.ckeConfig = {
      height: '550px',
      width: '780px',
      styles: { 'color': '#000000' },
      allowedContent: false,
      forcePasteAsPlainText: true,
      editorConfig: 'pt-br',
      removePlugins: 'placeholder_select, placeholder',
      toolbarGroups: [
        { name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
        { name: 'styles', groups: [ 'styles' ] },
        { name: 'colors', groups: [ 'colors' ] },
        { name: 'clipboard', groups: [ 'clipboard', 'undo' ] },
        { name: 'paragraph', groups: [ 'list', 'indent', 'blocks', 'align', 'bidi', 'paragraph' ] },
        { name: 'editing', groups: [ 'find', 'selection', 'spellchecker', 'editing' ] },
        { name: 'forms', groups: [ 'forms' ] },
        { name: 'insert', groups: [ 'insert' ] },
        { name: 'links', groups: [ 'links' ] },
        { name: 'tools', groups: [ 'tools' ] },
        { name: 'others', groups: [ 'others' ] },
        { name: 'about', groups: [ 'about' ] },
        { name: 'document', groups: [ 'mode', 'document', 'doctools' ] }
      ],
      removeButtons: 'Save,NewPage,Preview,Print,Templates,PasteText,PasteFromWord,SelectAll,Scayt,Form,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,CopyFormatting,Outdent,Indent,Blockquote,CreateDiv,BidiLtr,BidiRtl,Language,Anchor,Flash,Smiley,Iframe,Styles,About,Format,Subscript,Superscript,Strike'
    };
  }

  LoadMascara(Id: string) {
    if (Id != "0") {
      this.mascaraLaudoservice.ListaMascaraLaudo(Id).subscribe(
        mascaraLaudo => {
          if (!mascaraLaudo)
            this.mascaraLaudoservice.showToast(
              "warning",
              "Nenhuma Máscara Laudo localizada."
            );

          this.mascaraLaudo = mascaraLaudo;
        },
        erro => this.mascaraLaudoservice.showToast("error", erro)
      );
    }
  }

  Cancelar() {
    this.router.navigate(["/pages/mascaraLaudos/mascaraLaudo-grid"]);
  }

  Salvar() {
    this.LimparErros();

    this.mascaraLaudoservice.salva(this.mascaraLaudo).subscribe(
      msg => {
        if (!msg.erro) {
          if (!this.mascaraLaudo.Id && msg.id) {
            this.mascaraLaudo.Id = msg.id;
            this.listaMascara.push(this.mascaraLaudo);
          }
          this.mascaraLaudoservice.showToast(
            "success",
            "Máscara de laudo salva com sucesso!"
          );
          this.router.navigate(["/pages/mascaraLaudos/mascaraLaudo-grid"]);
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
          this.mascaraLaudoservice.showToast("warning", "Aceso negado.");
        }
        this.mascaraLaudoservice.showToast(
          "error",
          "Não foi possível salvar a máscara de laudo!"
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
          case "modalidade":
          this.erroModalidade = validationErrorDictionary[fieldName];
          break;
          default:
            this.mascaraLaudoservice.showToast("error", validationErrorDictionary[fieldName]);
            break;
        }
      }
    }
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      console.log(erro);
      this.mascaraLaudoservice.showToast(
        "error",
        erro
      );
    });
  }
}
