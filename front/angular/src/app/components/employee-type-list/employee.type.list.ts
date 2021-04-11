import {Component} from "@angular/core";
import {EmployeeTypeService} from "../../services/employee-type.service";
import {EmployeeType} from "../../models/employeeType";

@Component({
    templateUrl: "employee.type.list.html",
    selector: 'app-employee-type-list'
})
export class EmployeeTypeListComponent {
    employeeTypes: EmployeeType[];

    constructor(private employeeTypeService: EmployeeTypeService) {
        this.employeeTypes = [];
        this.get();
    }

    async get(): Promise<void> {
        this.employeeTypes = await this.employeeTypeService.get();
    }
}