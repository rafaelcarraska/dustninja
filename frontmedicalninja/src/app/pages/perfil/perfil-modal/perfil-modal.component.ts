import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { IActionMapping, ITreeOptions } from "angular-tree-component";
import { AcessoComponent } from "../../../@core/model/acesso/acesso.component";
import { TelaComponent } from "../../../@core/model/tela/tela.component";
import { NodesNinjaComponent } from "../../../@core/model/tree-nodes/treenodesninja.component";
import { EmpresaService } from "./../../../@core/data/empresa.service";
import { PerfilService } from "./../../../@core/data/perfil.service";
import { PerfilComponent } from "./../perfil.component";


@Component({
  moduleId: module.id,
  selector: "perfil-modal",
  templateUrl: "./perfil-modal.component.html",
  styleUrls: [
    "../../../@theme/styles/modal.scss",
    "./perfil-modal.component.scss"
  ]
})
export class PerfilModalComponent {
  nodes = [];

  modalHeader: string = "";
  perfil: PerfilComponent = new PerfilComponent();
  listaPerfis: PerfilComponent[] = [];
  perfilservice: PerfilService;
  empresaservice: EmpresaService;

  errors: string[];
  erroDescricao: string;

  constructor(
    private activeModal: NgbActiveModal,
    perfilservice: PerfilService,
    empresaservice: EmpresaService
  ) {
    this.perfilservice = perfilservice;
    this.empresaservice = empresaservice;

    this.carregaPermissoes();

  }

  actionMapping: IActionMapping = {
    mouse: {
      click: (tree, node) => this.check(node, !node.data.checked)
    }
  };

  options: ITreeOptions = {
    actionMapping: this.actionMapping
  };

  public check(node, checked) {
    this.updateChildNodeCheckbox(node, checked);
    this.updateParentNodeCheckbox(node.realParent);
  }
  public updateChildNodeCheckbox(node, checked) {
    node.data.checked = checked;
    if (node.children) {
      node.children.forEach((child) => this.updateChildNodeCheckbox(child, checked));
    }
  }
  public updateParentNodeCheckbox(node) {
    if (!node) {
      return;
    }

    let allChildrenChecked = true;
    let noChildChecked = true;

    for (const child of node.children) {
      if (!child.data.checked || child.data.indeterminate) {
        allChildrenChecked = false;
      }
      if (child.data.checked) {
        noChildChecked = false;
      }
    }

    if (allChildrenChecked) {
      node.data.checked = true;
      node.data.indeterminate = false;
    } else if (noChildChecked) {
      node.data.checked = false;
      node.data.indeterminate = false;
    } else {
      node.data.checked = true;
      node.data.indeterminate = true;
    }
    this.updateParentNodeCheckbox(node.parent);
  }

  carregaPermissoes() {
    this.empresaservice.listaEmpresa().subscribe(
      listatelas => {
        if (!listatelas) {
          this.perfilservice.showToast(
            "warning",
            "Nenhuma Permissão localizadas para essa empresa."
          );
        } else {
          this.montaTreeNodes(listatelas.listaTela);
        }
      },
      erro => this.perfilservice.showToast("error", erro)
    );
  }

  montaTreeNodes(listaTela: TelaComponent[]) {
    let listaNodes: NodesNinjaComponent[] = [];
    let treeNodes: NodesNinjaComponent;
    listaTela.forEach(tela => {
      if (!tela.master) {
        treeNodes = new NodesNinjaComponent();
        treeNodes.name = tela.descricao;
        let telaPerfil = this.perfil.acesso.listaTela.filter(x => x.descricao == tela.descricao);
        if (telaPerfil.length > 0)
          treeNodes.checked = true;
        else
          treeNodes.checked = false;

        treeNodes.children = this.montaTreeNodesFilho(tela.permissao, telaPerfil);

        listaNodes.push(treeNodes);
      }
    });

    this.nodes = listaNodes;
  }

  montaTreeNodesFilho(listaPermissao: string[], tela: TelaComponent[]) {
    let listaNodes: NodesNinjaComponent[] = [];
    let treeNodes: NodesNinjaComponent;
    listaPermissao.forEach(permissao => {
      treeNodes = new NodesNinjaComponent();
      treeNodes.name = permissao;

      if (this.perfil.acesso.listaTela) {
        if (tela.length > 0) {
          if (tela[0].permissao.filter(x => x == permissao).length > 0)
            treeNodes.checked = true;
          else
            treeNodes.checked = false;
        }
      } else {
        treeNodes.checked = false;
      }

      listaNodes.push(treeNodes);
    });
    return listaNodes;
  }

  closeModal() {
    this.activeModal.close();
  }

  SalvarPermissoes() {
    this.perfil.acesso = new AcessoComponent();
    this.nodes.forEach(listaTelas => {
      if (listaTelas.checked) {
        let tela = new TelaComponent();
        tela.descricao = listaTelas.name;
        tela.permissao = this.SalvarPermissoesFilho(listaTelas.children);

        this.perfil.acesso.listaTela.push(tela);
      }
    });
  }

  SalvarPermissoesFilho(children: NodesNinjaComponent[]) {
    let permissoes: string[] = [];
    children.forEach(listaPermissoes => {
      if (listaPermissoes.checked)
        permissoes.push(listaPermissoes.name);
    })
    return permissoes;
  }

  Salvar() {
    this.LimparErros();

    this.SalvarPermissoes();

    this.perfilservice.salva(this.perfil).subscribe(

      msg => {
        if (!msg.erro) {
          if (!this.perfil.Id && msg.id) {
            this.perfil.Id = msg.id;
            this.listaPerfis.push(this.perfil);
          }
          this.perfilservice.showToast("success", "Perfil salvo com sucesso!");
          this.closeModal();
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
          this.perfilservice.showToast("warning", "Aceso negado.");
        }
        this.perfilservice.showToast(
          "error",
          "Não foi possível salvar o perfil!"
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
          this.perfilservice.showToast("error", validationErrorDictionary[fieldName]);
            break;
        }
      }
    }
  }

  ShowErrors(errors) {
    errors.forEach(erro => {
      console.log(erro);
      this.perfilservice.showToast(
        "error",
        erro
      );
    });
  }

}


