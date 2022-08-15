import { FileDCMComponent } from '../../../@core/model/fileDCM/fileDCM.component';
import { environment } from "../../../../environments/environment";
import { HttpClient, HttpEventType, HttpRequest } from "@angular/common/http";
import { Component, Input, ViewChild, ElementRef } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { LoginService } from "../../../@core/data/login.service";
import { WorklistService } from "../../../@core/data/worklist.service";
import { AnexoComponent } from '../../../@core/model/anexos/anexos.component';
import { FileDCMNotaComponent } from '../../../@core/model/NotaHistorico/fileDCMNota.component';
import { Lightbox, IAlbum } from 'ngx-lightbox';

@Component({

  moduleId: module.id,
  selector: "./worklist-anexos",
  templateUrl: "./worklist-anexos.component.html",
  styleUrls: ["./worklist-anexos.component.scss",
    "../../../@theme/styles/modal.scss"
  ]
})
export class WorklistAnexosComponent {
  @Input()
  modalHeader: string = "";
  fileDCM: FileDCMComponent = new FileDCMComponent();
  recent: any[];
  listaAnexos: AnexoComponent[];
  fileDCMNota: FileDCMNotaComponent = new FileDCMNotaComponent();
  usuarioId: string = environment.usuarioId;
  public progress: number;
  worklistservice: WorklistService;
  anexoImagem: string = "";
  viewAnexo: boolean = false;
  public albums: Array<any>;

  constructor(
    private activeModal: NgbActiveModal,
    worklistservice: WorklistService,
    private loginservice: LoginService,
    private http: HttpClient,
    private _lightbox: Lightbox
  ) {
    this.worklistservice = worklistservice;
    this.albums = [];
  }

  ngOnInit() {
    this.modalHeader = `Anexar Documentos - ${this.fileDCM.pacienteNome} - ${this.fileDCM.studyDesc} - ${this.fileDCM.body_part} - ${this.fileDCM.date_study_formatada}`
    this.LoadAnexos();

    // console.log('listaAnexos', this.listaAnexos);
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

    for (let file of files) formData.append(this.fileDCM.Id, file);

    const req = new HttpRequest(
      "POST",
      environment.serviceUrl + `Upload/Anexo`,
      formData,
      {
        reportProgress: true
      }
    );

    this.http.request(req).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round((100 * event.loaded) / event.total);
      }
      else if (event.type === HttpEventType.Response) {
        // this.anexo.arquivo = event.body["id"];
        if (event.body["erro"]) {
          this.ShowErrors(event.body["erro"]);
        } else {
          this.worklistservice.showToast(
            "success",
            "Upload realizado com sucesso!"
          );
        }
        this.LoadAnexos();
      }
    });
  }

  LoadAnexos() {
    this.fileDCMNota.fileDCMId = this.fileDCM.Id;
    this.worklistservice.listaanexos(this.fileDCM.Id).subscribe(
      listaAnexos => {
        if (listaAnexos) {
          this.listaAnexos = listaAnexos;
        } else {
          this.listaAnexos = [];
        }
        this.fileDCM.countAnexo = this.listaAnexos.length;
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
        // console.log('albums', this.albums);
      },
      erro => {
        console.log(erro);
        this.worklistservice.erro(erro);
      }
    );
  }

  closeModal() {
    this.activeModal.close();
  }

  DeletarAnexo(anexo: AnexoComponent) {
    if (confirm("Deseja realmente excluir o arquivo?")) {
      this.worklistservice.DeletarAnexo(anexo).subscribe(
        msg => {
          if (!msg.erro) {
            let novoAnexo = this.listaAnexos.slice(0);
            let indice = novoAnexo.indexOf(anexo);
            novoAnexo.splice(indice, 1);
            this.listaAnexos = novoAnexo;
            this.fileDCM.countAnexo--;

            let novoAlbums = this.albums.slice(0)
            let album = this.albums.filter(x => x.Id == anexo.Id);
            let indiceAlbum = novoAlbums.indexOf(album);
            novoAlbums.splice(indiceAlbum, 1);
            this.albums = novoAlbums;

            this.worklistservice.showToast(
              "success",
              "Anexo removido com sucesso!"
            );
          } else {
            this.ShowErrors(msg.erro);
          }
        },
        erro => {
          this.worklistservice.showToast("warning", erro);
          if (erro.status === 401 || erro.status === 403) {
            this.worklistservice.showToast("warning", "Aceso negado.");
          }
          this.worklistservice.showToast(
            "error",
            "Não foi possível remover o anexo!"
          );
        }
      );
    } else {
      return false;
    }
  }

  ShowErrors(errors) {
    if (errors) {
      errors.forEach(erro => {
        this.worklistservice.showToast("warning", erro);
      });
    }
  }

  funcaoAnexo(extensao: string) {
    switch (extensao) {
      case ".png": return "fa-file-image";
      case ".jpg": return "fa-file-image";
      case ".jpeg": return "fa-file-image";
      case ".pdf": return "fa-file-pdf";
      case ".docx": return "fa-file-word";
      case ".doc": return "fa-file-word";
      case ".xlsx": return "fa-file-excel";
      case ".mp4": return "fa-file-video";
      case ".3gp": return "fa-file-audio";
      case ".zip": return "fa-file-archive";
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
