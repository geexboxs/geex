import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  $loading: Subject<boolean> = new BehaviorSubject(true);
  constructor() {}
}
