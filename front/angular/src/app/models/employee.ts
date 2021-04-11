import Util from "../util/util";
import {EmployeeType} from "./employeeType";

export class Employee {
    id: string;
    name: string;
    employmentDate: Date;
    salaryBase: number;
    employeeType?: EmployeeType;
    chief?: string;

    constructor() {
        this.id = Util.getEmptyId();
        this.name = "";
        this.salaryBase = 1000;
        this.employmentDate = new Date();
    }

    static fromData(other: any): Employee {
        let employee = new Employee();
        employee.id = other.id || employee.id;
        employee.name = other.name || employee.name;
        employee.salaryBase = other.salaryBase && Number(other.salaryBase) || employee.salaryBase;
        employee.employmentDate =
            other.employmentDate && new Date(other.employmentDate) || employee.employmentDate;
        employee.employeeType =
            other.employeeType && EmployeeType.fromData(other.employeeType) || undefined;
        employee.chief = other.chief || employee.chief;
        return employee;
    }

    static fromDataList(other: any[]): Employee[] {
        return other.map(
            (employee: any, _index: Number, _employees: any[]) =>
                Employee.fromData(employee));
    }
}
