import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import WorkerType from '../models/worker.type';

@Injectable()
export class WorkerTypeService {
    private readonly backend: string;

    constructor(private http: HttpClient) {
        this.backend = 'api/workerType';
    }

    get(): Observable<WorkerType[]> {
        return this.http
            .get<WorkerType[]>(this.backend);
    }
}