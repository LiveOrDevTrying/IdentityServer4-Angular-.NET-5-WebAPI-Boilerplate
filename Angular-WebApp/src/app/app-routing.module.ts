import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DataComponent } from './data/data.component';
import { HomeComponent } from './home/home.component';
import { AuthguardService } from './services/authguard.service';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'data', component: DataComponent, canActivate: [AuthguardService], runGuardsAndResolvers: "always" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
