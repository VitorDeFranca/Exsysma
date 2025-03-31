import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Constants } from '../utils/constants';
import { Rule } from '../models/rule';

@Injectable({
  providedIn: 'root'
})
export class RuleService {
  private apiUrl = `${Constants.apiPort}/rules`;

  constructor(private http: HttpClient) { }

  getRules(projectId?: number): Observable<Rule[]> {
    let url = this.apiUrl;
    if (projectId) {
      url += `?projectId=${projectId}`;
    }
    return this.http.get<Rule[]>(url);
  }

  getRule(id: number): Observable<Rule> {
    return this.http.get<Rule>(`${this.apiUrl}/${id}`);
  }

  createRule(rule: Partial<Rule>): Observable<Rule> {
    return this.http.post<Rule>(this.apiUrl, rule);
  }

  updateRule(id: number, rule: Partial<Rule>): Observable<Rule> {
    return this.http.put<Rule>(`${this.apiUrl}/${id}`, rule);
  }

  deleteRule(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}