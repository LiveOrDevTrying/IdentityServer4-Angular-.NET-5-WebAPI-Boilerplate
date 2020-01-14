import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OAuthModule } from 'angular-oauth2-oidc';

import { AppRoutingModule } from 'src/app/app-routing.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    OAuthModule.forRoot(),
    AppRoutingModule
  ]
})
export class AuthModule { }
