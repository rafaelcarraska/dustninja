import { ErrorHandler, Injectable, Injector } from "@angular/core";
import { Router } from "@angular/router";

@Injectable()
export class CustomErrorHandler implements ErrorHandler {
  constructor(private readonly injector: Injector) { }

  handleError(error: any) {
    const router = this.injector.get(Router);

    if (!error.status) { return; }

    switch (error.status) {
      case 401: { router.navigate(["/auth/login"]); }
    }
  }
}
