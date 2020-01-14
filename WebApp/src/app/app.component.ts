import { Component, OnInit, OnDestroy } from '@angular/core';
import { OAuthService, JwksValidationHandler, OAuthEvent } from 'angular-oauth2-oidc';
import { GlobalsService } from './core/services';
import { Title } from '@angular/platform-browser';
import { Router, NavigationEnd } from '@angular/router';
import { authConfig } from './core/auth';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  claims: any;

  $oauthSubscription: Subscription;

  constructor(
    private oauthService: OAuthService,
    private globalsService: GlobalsService,
    private titleService: Title,
    private router: Router) {
      this.router.events.subscribe(event => {
        // This is for Google Analytics
        if (event instanceof NavigationEnd) {
          (<any>window).ga('set', 'page', event.urlAfterRedirects);
          (<any>window).ga('send', 'pageview');
        }
      });

      this.$oauthSubscription = this.oauthService.events.subscribe((event: OAuthEvent) => {
        const accessToken = this.oauthService.getAccessToken();

        // Check to see if you have the token, if yes and not connected to webhub, connect
      });

      this.configureWithNewConfigApi();
  }

  ngOnInit() {
    this.oauthService.postLogoutRedirectUri = this.globalsService.landingPageUri;
    const accessToken = this.oauthService.getAccessToken();

    if (accessToken !== undefined &&
      accessToken !== null &&
      accessToken !== '' &&
      this.isAuthenticated) {
        // Connect to SignalR here, user is authorized
      }

      this.setTitle("The Monitaur - Monitor All Your WebApps / WebAPIs / Services From 1 Location");
  }

  ngOnDestroy() {
    this.$oauthSubscription.unsubscribe();
  }

  setTitle(newTitle: string) {
    this.titleService.setTitle(newTitle);
  }

  private configureWithNewConfigApi() {
    this.oauthService.configure(authConfig);
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
