import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable()
export class SalaryService {
    private readonly backend: string;

    constructor(private http: HttpClient) {
        this.backend = 'api/v1.0/salary';
    }

    get(date: Date): Observable<number> {
        return this.http
            .get<number>(this.backend, {
                params: {
                    date: date.toISOString()
                }
            });
    }

    getById(date: Date, employeeId: string): Observable<number> {
        return this.http
            .get<number>(this.backend, {
                params: {
                    date: date.toISOString(),
                    employeeId: employeeId
                }
            });
    }
}