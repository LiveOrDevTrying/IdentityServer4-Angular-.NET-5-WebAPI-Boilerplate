import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GlobalsService {

  constructor() { }

  IdentityServer_Http_URI: string = 'http://localhost:5000';
  IdentityServer_Https_URI: string = 'https://localhost:5001';
  WebAPI_Http_URI: string = 'http://localhost:5002';
  WebAPI_Https_URI: string = 'https://localhost:5003';
  WebApp_URI: string = 'http://localhost:4200';
  WebApp_Redirect_URI: string = 'http://localhost:4200';
  WebApp_Post_Logout_Redirect_URI: string = 'http://localhost:4200';

  Client_Id: string = 'AngularClient';
  Client_Scopes: string = 'openid profile roles api.resource.scope';
}
