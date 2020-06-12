import {Component, Inject} from "@angular/core";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Employee} from "../../models/employee";
import Util from "../../util/util";
import {EmployeeService} from "../../services/employee.service";
import {Observable} from "rxjs";
import {EmployeeTypeService} from "../../services/employee-type.service";
import {EmployeeType} from "../../models/employeeType";
import {FormControl} from "@angular/forms";

@Component({
    selector: 'app-edit-employee-dialog',
    templateUrl: 'edit.employee.dialog.html'
})
export class EditEmployeeDialogComponent {
    state: string;
    employeeTypes: EmployeeType[];
    chiefs: Employee[];
    employmentDateControl: FormControl;
    employeeTypeControl: FormControl;
    chiefControl: FormControl;

    constructor(
        public dialogRef: MatDialogRef<EditEmployeeDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public employee: Employee,
        private employeeService: EmployeeService,
        private employeeTypeService: EmployeeTypeService) {
        this.employmentDateControl = new FormControl(employee.employmentDate);
        this.employeeTypeControl = new FormControl(employee.employeeType?.id);
        this.chiefControl = new FormControl(employee.chief);
        if (employee.id === Util.getEmptyId()) {
            this.state = 'Add';
        } else {
            this.state = 'Edit';
        }
        this.getEmployeeTypes();
        this.getChiefs();
    }

    onNameInput(value: string): void {
        this.employee.name = value;
    }

    onEmploymentDateChange(date: string): void {
        this.employee.employmentDate = new Date(date);
        this.getChiefs();
    }

    onSalaryBaseInput(value: string): void {
        this.employee.salaryBase = Number(value);
    }

    onSaveClick(): void {
        let observable: Observable<Employee>;
        if (this.state === 'Add') {
            observable = this.employeeService.add(this.employee);
        } else {
            observable = this.employeeService.update(this.employee);
        }
        observable.subscribe({
            next: (employee) => {
                this.employee = employee;
                this.dialogRef.close(employee);
            },
            error: (error) => console.error(error)
        });
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    onEmployeeTypeChange(value: string): void {
        this.employee.employeeType = this.employeeTypes.find(employeeType => employeeType.id === value);
    }

    onChiefChange(value: string): void {
        this.employee.chief = value.length > 0 ? value : undefined;
    }

    private getEmployeeTypes(): void {
        this.employeeTypeService.get()
            .subscribe({
                next: (employeeTypes) => {
                    this.employeeTypes = EmployeeType.fromDataList(employeeTypes);
                    this.employee.employeeType = this.employee.employeeType
                        ? this.employeeTypes.find(employeeType => employeeType.id === this.employee.employeeType.id)
                        : this.employeeTypes[0];
                    this.employeeTypeControl.setValue(this.employee?.employeeType?.id);
                },
                error: (error) => console.error(error)
            });
    }

    private getChiefs(): void {
        this.employeeService.get(this.employee.employmentDate)
            .subscribe({
                next: (employees) => {
                    this.chiefs = employees;
                    this.chiefs.unshift(undefined);
                    this.chiefControl.setValue(this.employee?.chief);
                },
                error: (error) => console.error(error)
            });
    }
}