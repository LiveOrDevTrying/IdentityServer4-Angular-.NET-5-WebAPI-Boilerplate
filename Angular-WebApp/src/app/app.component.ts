import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService, OAuthEvent } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { Subscription } from 'rxjs';
import { AuthConfig } from 'angular-oauth2-oidc';
import { GlobalsService } from './services/globals.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  claims: any;
  accessToken = '';

  $oauthSubscription: Subscription;

  authConfig: AuthConfig = {
    issuer: this.globalsService.IdentityServer_Https_URI,
    redirectUri: window.location.origin,
    clientId: this.globalsService.Client_Id,
    scope: this.globalsService.Client_Scopes,
    postLogoutRedirectUri: this.globalsService.WebApp_Post_Logout_Redirect_URI
  }

  constructor(private oauthService: OAuthService,
    private router: Router,
    private globalsService: GlobalsService) {
    this.$oauthSubscription = this.oauthService.events.subscribe((event: OAuthEvent) => {
      this.accessToken = this.oauthService.getAccessToken();
    });
  }

  ngOnInit() {
    this.configureWithNewConfigApi();
  }

  ngOnDestroy() {
    this.$oauthSubscription.unsubscribe();
  }

  private configureWithNewConfigApi() {
    this.oauthService.configure(this.authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();

    this.oauthService.loadDiscoveryDocumentAndTryLogin().then(_ => {
      if (!this.oauthService.hasValidIdToken() ||
        !this.oauthService.hasValidAccessToken()) {
        this.oauthService.initImplicitFlow();
      } else {
        this.router.navigate(['/']);
      }
    })
  }

  logout() {
    this.oauthService.logOut();
  }

  get isAuthenticated(): boolean {
    this.claims = this.oauthService.getIdentityClaims();
    return this.claims !== undefined &&
      this.claims !== null;
  }
}

