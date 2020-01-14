import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthModule } from './auth';
import { HttpClientModule } from '@angular/common/http';
import { ServicesModule } from './services';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    AuthModule,
    ServicesModule,
    HttpClientModule
  ],
  exports: [
    CommonModule,
    AuthModule,
    ServicesModule,
    HttpClientModule
  ]
})
export class CoreModule { }
