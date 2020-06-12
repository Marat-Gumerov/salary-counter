import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Employee} from '../models/employee';
import {map} from "rxjs/operators";

@Injectable()
export class EmployeeService {
    private backend: string;

    constructor(private http: HttpClient) {
        this.backend = 'api/employee';
    }

    get(date: Date): Observable<Employee[]> {
        let dateString: string;
        if (date instanceof Date) {
            dateString = date.toISOString();
        } else {
            dateString = new Date(date).toISOString();
        }
        return this.http
            .get<Employee[]>(this.backend, {
                params: {
                    selectionDate: dateString
                }
            })
            .pipe(map(employees => Employee.fromDataList(employees)));
    }

    getById(id: string): Observable<Employee> {
        const url = `${this.backend}/${id}`;
        return this.http
            .get<Employee>(url)
            .pipe(map(employee => Employee.fromData(employee)));
    }

    add(employee: Employee): Observable<Employee> {
        return this.http
            .post<Employee>(this.backend, employee)
            .pipe(map(employee => Employee.fromData(employee)));
    }

    update(employee: Employee): Observable<Employee> {
        return this.http
            .put<Employee>(this.backend, employee)
            .pipe(map(employee => Employee.fromData(employee)));
    }

    delete(id: string): Observable<{}> {
        const url = `${this.backend}/${id}`;
        return this.http
            .delete(url);
    }
}