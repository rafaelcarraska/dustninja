import { ToasterConfig } from "angular2-toaster";

export const environment = {
  production: true,
  serviceUrl:"https://api.dustmedical.ninja/",
  serviceUrlDCM:"https://dcm.dustmedical.ninja/",
  serviceUrlOHIF:"https://ohif.dustmedical.ninja/",
  serviceUrlFile:"https://health.dustmedical.ninja/",
  usuarioId:"",
  master: false,
  serviceUrlOsirix:"osirix://?methodName=downloadURL&URL=",
  serviceUrlHorus:"horus://?methodName=downloadURL&URL=",
  viewOsirix: false,
  viewHoros: false,
  beta: true,
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
