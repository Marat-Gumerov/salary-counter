import {Component, EventEmitter, Input, Output} from "@angular/core";
import {SalaryService} from "../../services/salary.service";
import {Employee} from "../../models/employee";
import {MatDialog} from "@angular/material/dialog";
import {EditEmployeeDialogComponent} from "../edit-employee-dialog/edit.employee.dialog";
import {EmployeeService} from "../../services/employee.service";

@Component({
    templateUrl: "employee.html",
    selector: 'app-employee'
})
export class EmployeeComponent {
    @Input() employee: Employee;
    @Input() date: Date;
    @Output() onDelete: EventEmitter<Employee> = new EventEmitter<Employee>();
    salary: number;

    constructor(
        private salaryService: SalaryService,
        private employeeService: EmployeeService,
        public dialog: MatDialog) {
        this.salary = -1;
        this.employee = new Employee();
        this.date = new Date();
    }

    async onGetSalaryClick(): Promise<void> {
        this.salary = await this.salaryService.getById(this.date, this.employee.id);
    }

    async onEdit(): Promise<void> {
        const result = await this.dialog.open(EditEmployeeDialogComponent, {
            data: this.employee
        }).afterClosed().toPromise();
        this.employee = result ?? this.employee;
    }

    async delete(): Promise<void> {
        await this.employeeService.delete(this.employee.id);
        this.onDelete.emit(this.employee);
    }
}