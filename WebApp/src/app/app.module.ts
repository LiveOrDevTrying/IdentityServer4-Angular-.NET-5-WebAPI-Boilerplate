import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { FeaturesModule } from './features';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    FeaturesModule,
    AppRoutingModule,
  ],
  exports: [
    AppRoutingModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
