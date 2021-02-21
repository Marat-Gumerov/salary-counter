import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {EmployeeType} from '../models/employeeType';

@Injectable()
export class EmployeeTypeService {
    private readonly backend: string;

    constructor(private http: HttpClient) {
        this.backend = 'api/v1.0/employeeType';
    }

    async get(): Promise<EmployeeType[]> {
        return await this.http
            .get<EmployeeType[]>(this.backend).toPromise();
    }
}