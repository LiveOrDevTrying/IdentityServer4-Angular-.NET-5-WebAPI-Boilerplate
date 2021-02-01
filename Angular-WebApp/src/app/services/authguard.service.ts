import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthguardService implements CanActivate {

  constructor(
    private oauthService: OAuthService,
    private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    // Check to see if we have an application user
    // If we dont, we navigate to [/]
    if (this.oauthService.hasValidAccessToken()) {
      return true;
    }

    this.router.navigate(['/']);
    return false;
  };
}
