import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Employee} from '../models/employee';
import {map} from "rxjs/operators";

@Injectable()
export class EmployeeService {
    private readonly backend: string;

    constructor(private http: HttpClient) {
        this.backend = 'api/v1.0/employee';
    }

    async get(date: Date): Promise<Employee[]> {
        let dateString = date instanceof Date ? date.toISOString() : new Date(date).toISOString();
        return await this.http
            .get<Employee[]>(this.backend, {
                params: {
                    selectionDate: dateString
                }
            })
            .pipe(map(employees => Employee.fromDataList(employees)))
            .toPromise();
    }

    async getById(id: string): Promise<Employee> {
        const url = `${this.backend}/${id}`;
        return await this.http
            .get<Employee>(url)
            .pipe(map(employee => Employee.fromData(employee)))
            .toPromise();
    }

    async add(employee: Employee): Promise<Employee> {
        return await this.http
            .post<Employee>(this.backend, employee)
            .pipe(map(employee => Employee.fromData(employee)))
            .toPromise();
    }

    async update(employee: Employee): Promise<Employee> {
        return await this.http
            .put<Employee>(this.backend, employee)
            .pipe(map(employee => Employee.fromData(employee)))
            .toPromise();
    }

    async delete(id: string): Promise<void> {
        const url = `${this.backend}/${id}`;
        await this.http
            .delete(url)
            .toPromise();
    }
}