import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
  {
    title: 'WorkList',
    icon: 'nb-compose',
    link: '/pages/worklist',
    home: true,
  },{
    title: 'Unidades',
    icon: 'fa fa-hospital',
    link: '/pages/facility/facility-grid',
  },{
    title: 'Filtros',
    icon: 'fa fa-filter iconFilter',
    link: '/pages/filtros/filtros-grid',
  },{
    title: 'Máscaras de Laudo',
    icon: 'fa fa-notes-medical',
    link: '/pages/mascaraLaudos/mascaraLaudo-grid',
    hidden: false,
  },{
    title: 'Templates de Impressão',
    icon: 'fa fa-print',
    link: '/pages/templateImpressoes/templateImpressao-grid',
  },{
    title: 'Tipos de Exames',
    icon: 'fa fa-briefcase-medical ',
    link: '/pages/tipoExame/tipoExame-grid',
  },{
    title: 'Paciente',
    icon: 'fa fa-user ',
    link: '/pages/paciente/paciente-grid',
  },{
    title: 'Sistema',
    icon: 'nb-gear',
    children: [
      {
        title: 'Empresa',
        link: '/pages/empresa/empresa-grid',
      },{
        title: 'Usuários',
        link: '/pages/usuarios/usuario-grid',
      },{
        title: 'Perfis',
        link: '/pages/perfil/perfil-grid',
      },{
        title: 'Relatório',
        link: '/pages/relatorio/relatorio-gerencial',
      },
    ],
  },
];
