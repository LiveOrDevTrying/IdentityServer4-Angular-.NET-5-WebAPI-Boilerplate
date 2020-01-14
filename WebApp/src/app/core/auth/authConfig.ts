import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {
    issuer: 'http://localhost:5000',
    redirectUri: window.location.origin,
    clientId: 'phs.themonitaur.webapp',
    scope: 'openid profile roles phs.themonitaur.webapi',
    postLogoutRedirectUri: 'https://localhost:44335'
}