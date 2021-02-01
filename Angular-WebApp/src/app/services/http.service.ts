import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { GlobalsService } from './globals.service';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  response: string = '';
  private $apiSubject = new Subject<string>();

  constructor(private httpClient: HttpClient,
    private globalsService: GlobalsService) {
  }

  requestWeatherForecast() {
    this.httpClient.get<string>(`${this.globalsService.WebAPI_Https_URI}/WeatherForecast`)
      .subscribe((response: string) => {
        this.$apiSubject.next(JSON.stringify(response));
      });
  }

  getWeatherForecast(): Observable<string> {
    return this.$apiSubject.asObservable();
  }
}
