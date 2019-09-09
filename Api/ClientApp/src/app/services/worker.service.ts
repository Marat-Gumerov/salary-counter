import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import Worker from '../models/worker';

@Injectable()
export class WorkerService {
    private backend: string;
    constructor(private http: HttpClient) {
        this.backend = 'api/worker';
    }
    get(date: Date): Observable<Worker[]> {
        return this.http
            .get<Worker[]>(this.backend, {
                params: {
                    selectionDate: date.toISOString()
                }
            });
    }

    getById(id: string): Observable<Worker> {
        const url = `${this.backend}/${id}`;
        return this.http
            .get<Worker>(url);
    }

    add(worker: Worker): Observable<Worker> {
        return this.http
            .post<Worker>(this.backend, worker);
    }

    update(worker: Worker): Observable<Worker> {
        return this.http
            .put<Worker>(this.backend, worker);
    }

    delete(id: string): Observable<{}> {
        const url = `${this.backend}/${id}`;
        return this.http
            .delete(url);
    }
}