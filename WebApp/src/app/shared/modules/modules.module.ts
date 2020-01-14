import { NgModule } from '@angular/core';
import { MaterialModule } from './material.module';
import { CoreModule } from 'src/app/core';

@NgModule({
  declarations: [],
  imports: [
    CoreModule,
    MaterialModule
  ],
  exports: [
    CoreModule,
    MaterialModule
  ]
})
export class ModulesModule { }
