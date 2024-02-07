import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DistrictService {
  private apiUrl = 'https://api.data.gov.in/resource/9ef84268-d588-465a-a308-a864a43d0070';
  private apiKey = 'YOUR_API_KEY';  // Replace with your API key

  constructor(private http: HttpClient) { }

  getDistrictsByState(stateCode: string): Observable<string[]> {
    const params = new HttpParams()
      .set('api-key', this.apiKey)
      .set('format', 'json')
      .set('filters[state_code]', stateCode);

    return this.http.get<any>(this.apiUrl, { params })
      .pipe(
        map((response: { records: any[]; }) => response.records.map(record => record.district))
      );
  }
}
