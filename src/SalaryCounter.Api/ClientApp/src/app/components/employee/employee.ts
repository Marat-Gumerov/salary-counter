import {Component, Input} from "@angular/core";
import {SalaryService} from "../../services/salary.service";
import {Employee} from "../../models/employee";
import {MatDialog} from "@angular/material/dialog";
import {EditEmployeeDialogComponent} from "../edit-employee-dialog/edit.employee.dialog";

@Component({
    templateUrl: "employee.html",
    selector: 'app-employee'
})
export class EmployeeComponent {
    @Input() employee: Employee;
    @Input() date: Date;
    salary: number;

    constructor(
        private salaryService: SalaryService,
        public dialog: MatDialog) {
        this.salary = -1;
    }

    async onGetSalaryClick(): Promise<void> {
        this.salary = await this.salaryService.getById(this.date, this.employee.id);
    }

    onEdit(): void {
        const dialogRef = this.dialog.open(EditEmployeeDialogComponent, {
            data: this.employee
        })
        dialogRef.afterClosed()
            .subscribe(result => this.employee = result ?? this.employee);
    }
}