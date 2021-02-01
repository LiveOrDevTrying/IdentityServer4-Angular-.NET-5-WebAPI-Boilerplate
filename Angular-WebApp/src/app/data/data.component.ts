import { Component, OnDestroy, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Subscription } from 'rxjs';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-data',
  templateUrl: './data.component.html',
  styleUrls: ['./data.component.scss']
})
export class DataComponent implements OnInit, OnDestroy {
  data = '';

  $forecastSubscription: Subscription;

  constructor(private httpService: HttpService) {
    this.$forecastSubscription = this.httpService.getWeatherForecast()
      .subscribe((data: string) => {
        this.data = data;
      });
  }

  ngOnInit() {
    this.httpService.requestWeatherForecast();
  }

  ngOnDestroy() {
    this.$forecastSubscription.unsubscribe();
  }
}
