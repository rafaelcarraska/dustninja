import { HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable()
export class CustomHttpInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler) {
    let url = request.url;

    request = request.clone({
      setHeaders: { Authorization: `Bearer ${sessionStorage.getItem("token")}` },
      url
    });

    return next.handle(request);
  }
}
