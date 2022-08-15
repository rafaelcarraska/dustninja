import { Router } from "@angular/router";
import { Injectable } from '@angular/core';
import { Component, Input, OnInit } from '@angular/core';

@Component ({
    selector: 'filtro-basico',
    templateUrl: './filtro-basico.component.html',
    styleUrls: [
        "../../../@theme/styles/grid.scss",
        "./filtro-basico.component.scss"
    ]
})

@Injectable()
export class FiltroBasicoComponent {
    public searchString: string = '';
    @Input() saida: any;

  constructor(
    public readonly router: Router,
  ){

  }

  pager: any = {};
  pagedItems: any[];

  ngAfterViewInit() {
  }

  setfiltro(){
    this.saida.value = this.searchString;
    // console.log(this.saida);
  }

}
