import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {EmployeeType} from '../models/employeeType';

@Injectable()
export class EmployeeTypeService {
    private readonly backend: string;

    constructor(private http: HttpClient) {
        this.backend = 'api/employeeType';
    }

    get(): Observable<EmployeeType[]> {
        return this.http
            .get<EmployeeType[]>(this.backend);
    }
}