import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class SalaryService {
    private readonly backend: string;

    constructor(private http: HttpClient) {
        this.backend = 'api/v1.0/salary';
    }

    async get(date: Date): Promise<number> {
        return this.http
            .get<number>(this.backend, {
                params: {
                    date: date.toISOString()
                }
            })
            .toPromise();
    }

    async getById(date: Date, employeeId: string): Promise<number> {
        return this.http
            .get<number>(this.backend, {
                params: {
                    date: date.toISOString(),
                    employeeId: employeeId
                }
            })
            .toPromise();
    }
}