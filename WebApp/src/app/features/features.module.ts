import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { HomeComponent } from './home/home.component';
import { FeaturesRoutingModule } from './features-routing.module';

@NgModule({
  declarations: [HomeComponent],
  imports: [
    SharedModule,
    FeaturesRoutingModule,
  ],
  exports: [
    SharedModule,
    FeaturesRoutingModule,
  ]
})
export class FeaturesModule { }
