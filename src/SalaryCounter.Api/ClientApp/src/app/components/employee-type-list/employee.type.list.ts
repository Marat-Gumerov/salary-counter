import {Component} from "@angular/core";
import {EmployeeTypeService} from "../../services/employee-type.service";
import {EmployeeType} from "../../models/employeeType";

@Component({
    templateUrl: "employee.type.list.html",
    selector: 'app-employee-type-list'
})
export default class EmployeeTypeListComponent {
    employeeTypes: EmployeeType[];

    constructor(private employeeTypeService: EmployeeTypeService) {
        this.employeeTypes = [];
        this.get();
    }

    get(): void {
        this.employeeTypeService.get()
            .subscribe({
                next: (employeeTypes) => this.employeeTypes = employeeTypes,
                error: (error) => console.error(error)
            });
    }
}