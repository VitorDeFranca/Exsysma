import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Variable } from '../models/variable';
import { Constants } from '../utils/constants';

@Injectable({
  providedIn: 'root'
})
export class VariableService {
  private apiUrl = `${Constants.apiPort}/variables`;

  constructor(private http: HttpClient) { }

  getVariables(projectId?: number): Observable<Variable[]> {
    let url = this.apiUrl;
    if (projectId) {
      url += `?projectId=${projectId}`;
    }
    return this.http.get<Variable[]>(url);
  }

  getVariable(id: number): Observable<Variable> {
    return this.http.get<Variable>(`${this.apiUrl}/${id}`);
  }

  createVariable(variable: Partial<Variable>): Observable<Variable> {
    return this.http.post<Variable>(this.apiUrl, variable);
  }

  updateVariable(id: number, variable: Partial<Variable>): Observable<Variable> {
    return this.http.put<Variable>(`${this.apiUrl}/${id}`, variable);
  }

  deleteVariable(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}