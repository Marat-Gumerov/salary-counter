import {Component} from "@angular/core";
import {EmployeeService} from "../../services/employee.service";
import {Employee} from "../../models/employee";
import {MatDialog} from "@angular/material/dialog";
import {EditEmployeeDialogComponent} from "../edit-employee-dialog/edit.employee.dialog";
import {FormControl} from "@angular/forms";

@Component({
    selector: 'app-employee-list',
    templateUrl: "employee.list.html"
})
export class EmployeeListComponent {
    dateControl: FormControl;
    employees: Employee[];

    constructor(
        private employeeService: EmployeeService,
        public dialog: MatDialog) {
        this.dateControl = new FormControl(new Date());
        this.employees = [];
        this.get();
    }

    add(): void {
        const dialogRef = this.dialog.open(EditEmployeeDialogComponent, {
            data: new Employee()
        })
        dialogRef.afterClosed()
            .subscribe(() => this.get());
    }

    async get(): Promise<void> {
        this.employees = await this.employeeService.get(this.dateControl.value);
    }
}