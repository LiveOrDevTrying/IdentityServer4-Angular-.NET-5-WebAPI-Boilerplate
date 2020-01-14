import { NgModule } from '@angular/core';
import { CoreModule } from '../core/core.module';
import { ComponentsModule } from './components/components.module';
import { ModulesModule } from './modules';
import { DirectivesModule } from './directives';
import { InterceptorsModule } from './interceptors';
import { PipesModule } from './pipes/pipes.module';

@NgModule({
  declarations: [],
  imports: [
    CoreModule,
    ComponentsModule,
    DirectivesModule,
    InterceptorsModule,
    ModulesModule,
    PipesModule
  ],
  exports: [
    CoreModule,
    ComponentsModule,
    DirectivesModule,
    InterceptorsModule,
    ModulesModule,
    PipesModule
  ]
})
export class SharedModule { }
