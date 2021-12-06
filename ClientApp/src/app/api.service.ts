import {Observable} from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private httpClient: HttpClient) { }

  getTestData(): Observable<any> {
  //let url = `api/labs/UpdateUnReviewedLabs/practices/${this.sessionDetails.PracticePk}/patients/${data.patInfoPk}/review?labCodeHeaderPks=${data.labCodeHeaderPK}&reviewedByUserPk=${this.sessionDetails.UserProfilePk}&reviewedBy=${this.userDetails.lastName},${this.userDetails.firstName}`;
  const routePath = 'http://localhost:60417/api/Fs/samplecontents';
  return this.httpClient.get(routePath);    
  }
}
