import {Component, Input} from "@angular/core";
import {EmployeeType} from '../../models/employeeType';

@Component({
    templateUrl: "employee.type.html",
    selector: 'app-employee-type'
})
export class EmployeeTypeComponent {
    @Input() employeeType: EmployeeType | undefined;
}