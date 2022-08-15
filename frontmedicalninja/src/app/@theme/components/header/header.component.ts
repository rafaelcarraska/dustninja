import { environment } from './../../../../environments/environment';
import { Component, Input, OnInit } from '@angular/core';

import { NbMenuService, NbSidebarService } from '@nebular/theme';
import { UserService } from '../../../@core/data/users.service';
import { AnalyticsService } from '../../../@core/utils/analytics.service';
import { LoginService } from '../../../@core/data/login.service';

@Component({
  selector: 'ngx-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit {


  @Input() position = 'normal';

  user: any;
  dadosBascios: any;

  userMenu = [{ title: 'Sair', link: '../auth/login' }];

  constructor(private sidebarService: NbSidebarService,
              private menuService: NbMenuService,
              private userService: UserService,
              private loginservice: LoginService,
              private analyticsService: AnalyticsService) {
  }

  ngOnInit() {
    this.loginservice.DadosBasicos().subscribe(
      dadosBascios => {
        // console.log('dados básicos', dadosBascios);
        this.dadosBascios = dadosBascios;
        environment.master = dadosBascios.master;
        environment.usuarioId = this.dadosBascios.usuarioId;
      });
  }

  toggleSidebar(): boolean {
    this.sidebarService.toggle(true, 'menu-sidebar');
    return false;
  }

  toggleSettings(): boolean {
    this.sidebarService.toggle(false, 'settings-sidebar');
    return false;
  }

  goToHome() {
    this.menuService.navigateHome();
  }

  startSearch() {
    this.analyticsService.trackEvent('startSearch');
  }
}
