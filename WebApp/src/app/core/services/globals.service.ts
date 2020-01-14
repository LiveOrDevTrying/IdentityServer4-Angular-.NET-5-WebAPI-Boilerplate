import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GlobalsService {

  landingPageUri = 'https://localhost:44335';

  constructor() { }
}
