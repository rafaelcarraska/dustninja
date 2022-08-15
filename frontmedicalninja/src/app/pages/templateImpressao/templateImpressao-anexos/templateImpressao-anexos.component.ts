import { environment } from "../../../../environments/environment";
import { HttpClient, HttpEventType, HttpRequest } from "@angular/common/http";
import { Component, Input } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { LoginService } from "../../../@core/data/login.service";
import { AnexoTemplateComponent } from '../../../@core/model/anexosTemplate/anexosTemplate.component';
import { TemplateImpressaoComponent } from "../templateImpressao.component";
import { TemplateImpressaoService } from "../../../@core/data/templateImpressao.service";
import { Lightbox } from "ngx-lightbox";

@Component({
  moduleId: module.id,
  selector: "./templateImpressao-anexos",
  templateUrl: "./templateImpressao-anexos.component.html",
  styleUrls: [ "./templateImpressao-anexos.component.scss",
    "../../../@theme/styles/modal.scss"
  ]
})
export class TemplateAnexosComponent {
  @Input()
  modalHeader: string = "";
  templateImpressao: TemplateImpressaoComponent = new TemplateImpressaoComponent();
  recent: any[];
  listaAnexos: AnexoTemplateComponent[];
  usuarioId: string = environment.usuarioId;
  public progress: number;
  templateImpressaoservice: TemplateImpressaoService;
  anexoImagem: string = "";
  viewAnexo: boolean = false;
  public albums: Array<any>;

  constructor(
    private activeModal: NgbActiveModal,
    templateImpressaoservice: TemplateImpressaoService,
    private loginservice: LoginService,
    private http: HttpClient,
    private _lightbox: Lightbox

  ) {
    this.templateImpressaoservice = templateImpressaoservice;
    this.albums = [];
  }

  ngOnInit() {
    this.modalHeader = `Anexar Documentos - ${this.templateImpressao.descricao}`
    this.LoadAnexos();
  }

  open(index: number, usaLightbox: boolean): void {
    if (usaLightbox) {
      this._lightbox.open(this.albums.filter(x => x.usaLightbox == true), index);
    }
  }

  close(): void {
    this._lightbox.close();
  }

  upload(files) {
    if (files.length === 0) return;

    const formData = new FormData();

    for (let file of files) formData.append(this.templateImpressao.Id, file);

    const req = new HttpRequest(
      "POST",
      environment.serviceUrl + `Upload/AnexoTemplate`,
      formData,
      {
        reportProgress: true
      }
    );

    this.http.request(req).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress){
        this.progress = Math.round((100 * event.loaded) / event.total);
      }
      else if (event.type === HttpEventType.Response) {
        // this.anexo.arquivo = event.body["id"];
        if (event.body["erro"]) {
          this.ShowErrors(event.body["erro"]);
        } else {
          this.templateImpressaoservice.showToast(
            "success",
            "Upload realizado com sucesso!"
          );
        }
        this.LoadAnexos();
      }
    });
  }

  LoadAnexos() {
    this.templateImpressaoservice.listaAnexos().subscribe(
      listaAnexos => {
        if (listaAnexos) {
          this.listaAnexos = listaAnexos;
        }else{
          this.listaAnexos = [];
        }
        this.templateImpressao.countAnexo = this.listaAnexos.length;
        this.albums = [];
        for (let i = 0; i < this.listaAnexos.length; i++) {
          let album = {
            src: `${environment.serviceUrlFile}assets/images/files/${this.listaAnexos[i].arquivo}`,
            caption: this.listaAnexos[i].arquivo,
            thumb: `${environment.serviceUrlFile}assets/images/files/${this.listaAnexos[i].arquivo}`,
            Id: this.listaAnexos[i].Id,
            descricao: this.listaAnexos[i].descricao,
            usaLightbox: this.usaLightbox(this.listaAnexos[i].extensao),
            insertDataFormatada: this.listaAnexos[i].log.insertDataFormatada,
            tamanho: this.listaAnexos[i].tamanho,
            extensao: this.listaAnexos[i].extensao
          };
          if (album.extensao == '.pdf'){
            album.thumb = 'src/assets/anexo_files_preview/icon pdf file.png'
          }
          this.albums.push(album);
        }
      },
      erro => {
        console.log(erro);
        this.templateImpressaoservice.erro(erro);
      }
    );
  }

  closeModal() {
    this.activeModal.close();
  }

  copy(anexo: AnexoTemplateComponent){
    let selBox = document.createElement('textarea');
    selBox.style.position = 'fixed';
    selBox.style.left = '0';
    selBox.style.top = '0';
    selBox.style.opacity = '0';
    selBox.value = environment.serviceUrlFile + 'assets/images/files/'+ anexo.arquivo;
    document.body.appendChild(selBox);
    selBox.focus();
    selBox.select();
    document.execCommand('copy');
    document.body.removeChild(selBox);
  }

  DeletarAnexo(anexo: AnexoTemplateComponent){
    if (confirm("Deseja realmente excluir o arquivo?")) {
    this.templateImpressaoservice.DeletarAnexo(anexo).subscribe(
      msg => {
        if (!msg.erro) {
          let novoAnexo = this.listaAnexos.slice(0);
          let indice = novoAnexo.indexOf(anexo);
          novoAnexo.splice(indice, 1);
          this.listaAnexos = novoAnexo;
          this.templateImpressao.countAnexo--;

          let novoAlbums = this.albums.slice(0)
            let album = this.albums.filter(x => x.Id == anexo.Id);
            let indiceAlbum = novoAlbums.indexOf(album);
            novoAlbums.splice(indiceAlbum, 1);
            this.albums = novoAlbums;

          this.templateImpressaoservice.showToast(
            "success",
            "Anexo removido com sucesso!"
          );
        } else {
          this.ShowErrors(msg.erro);
        }
      },
      erro => {
        this.templateImpressaoservice.showToast("warning", erro);
        if (erro.status === 401 || erro.status === 403) {
          this.templateImpressaoservice.showToast("warning", "Aceso negado.");
        }
        this.templateImpressaoservice.showToast(
          "error",
          "Não foi possível remover o anexo!"
        );
      }
    );
  }else {
    return false;
  }
}

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.templateImpressaoservice.showToast("warning", erro);
      });
    }
  }

  funcaoAnexo(extensao: string){
    switch(extensao)
      {
        case ".png" : return "fa-file-image";
        case ".jpg" : return "fa-file-image";
        case ".jpeg" : return "fa-file-image";
        case ".pdf" : return "fa-file-pdf";
        case ".docx" : return "fa-file-word";
        case ".doc" : return "fa-file-word";
        case ".xlsx" : return "fa-file-excel";
        case ".mp4" : return "fa-file-video";
        case ".3gp" : return "fa-file-audio";
        case ".zip" : return "fa-file-archive";
      }

        return "fa-file";
    }
    
    usaLightbox(extensao: string) {
      switch (extensao) {
        case ".png": return true;
        case ".jpg": return true;
        case ".jpeg": return true;
      }
      return false;
    }
}
