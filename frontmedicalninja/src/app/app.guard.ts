import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import decode from "jwt-decode";

@Injectable()
export class AuthenticationGuard implements CanActivate {

  static hiddenMenu(): any {
    if (sessionStorage.getItem("token") != undefined) {
      return true;
    }
    return false;
  }
  constructor(private readonly router: Router) {}

  canActivate() {
    if (sessionStorage.getItem("token") != undefined) {
      return true;
    }
    this.router.navigate(["/auth/login"]);
    return false;
  }

  static roles(role: string): boolean {
    if (sessionStorage.getItem("token") != undefined) {
      const token = localStorage.getItem("token");
      const tokenPayload = decode(token);

      console.log(role)
      if (tokenPayload.role != role) {
        return false;
      }
      return true;
    }

    return false;
  }
}
