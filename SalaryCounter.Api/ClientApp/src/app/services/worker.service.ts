import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Worker} from '../models/worker';
import {map} from "rxjs/operators";

@Injectable()
export class WorkerService {
    private backend: string;

    constructor(private http: HttpClient) {
        this.backend = 'api/worker';
    }

    get(date: Date): Observable<Worker[]> {
        let dateString: string;
        if (date instanceof Date) {
            dateString = date.toISOString();
        } else {
            dateString = new Date(date).toISOString();
        }
        return this.http
            .get<Worker[]>(this.backend, {
                params: {
                    selectionDate: dateString
                }
            })
            .pipe(map(workers => Worker.fromDataList(workers)));
    }

    getById(id: string): Observable<Worker> {
        const url = `${this.backend}/${id}`;
        return this.http
            .get<Worker>(url)
            .pipe(map(worker => Worker.fromData(worker)));
    }

    add(worker: Worker): Observable<Worker> {
        return this.http
            .post<Worker>(this.backend, worker)
            .pipe(map(worker => Worker.fromData(worker)));
    }

    update(worker: Worker): Observable<Worker> {
        return this.http
            .put<Worker>(this.backend, worker)
            .pipe(map(worker => Worker.fromData(worker)));
    }

    delete(id: string): Observable<{}> {
        const url = `${this.backend}/${id}`;
        return this.http
            .delete(url);
    }
}