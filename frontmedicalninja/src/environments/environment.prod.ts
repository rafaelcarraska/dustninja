import { ToasterConfig } from "angular2-toaster";

export const environment = {
  production: true,
  serviceUrl:"https://api.fastpacs.com.br/",
  serviceUrlDCM:"https://dcm.fastpacs.com.br/",
  serviceUrlOHIF:"https://ohif.fastpacs.com.br/",
  serviceUrlFile:"https://fastpacs.com.br/",
  usuarioId:"",
  master: false,
  serviceUrlOsirix:"osirix://?methodName=downloadURL&URL=",
  serviceUrlHorus:"horus://?methodName=downloadURL&URL=",
  viewOsirix: false,
  viewHoros: false,
  beta: false,
  config: new ToasterConfig({
    positionClass: "toast-top-right",
    timeout: 6000,
    newestOnTop: true,
    tapToDismiss: true,
    preventDuplicates: true,
    animation: "flyLeft",
    limit: 5
  }),
};
