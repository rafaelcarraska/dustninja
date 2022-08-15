import { EnderecoComponent } from './../../pages/endereco/endereco.component';

import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';

@Injectable()
export class EnderecoService {

    public enderecoVM: EnderecoComponent = new EnderecoComponent();
    http: Http;
    headers: Headers;

    constructor(http: Http) {

        this.http = http;
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
    }

    buscarCEP(cep: string): Observable<JSON> {
        return this.http
        .get("https://viacep.com.br/ws/" + cep + "/json")
        .map(res => res.json());
    }
}


