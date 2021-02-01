import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class Httpinterceptor implements HttpInterceptor {
  constructor(private oauthService: OAuthService) { }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const accessToken = this.oauthService.getAccessToken();
    let updatedRequest: HttpRequest<any>;

    if (accessToken === undefined ||
      accessToken === null ||
      accessToken === '') {
      updatedRequest = request.clone();
    } else {
      updatedRequest = request.clone({
        headers: request.headers.append('Authorization', 'Bearer ' + this.oauthService.getAccessToken())
          .append('Access-Control-Allow-Methods', 'GET, POST, OPTIONS')
      });
    }

    return next.handle(updatedRequest).pipe(
      tap(
        event => {
          // log the httpResponse to the browser's console in case of success
          if (event instanceof HttpResponse) {

          }
        },
        error => {
          // log the httpResponse to the browser's console in case of failure
          if (event instanceof HttpResponse) {

          }
        }
      )
    )
  }
}
