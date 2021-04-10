import {SalaryRatio} from "./salary.ratio";
import Util from "../util/util";

export class EmployeeType {
    id: string;
    value: string;
    canHaveSubordinates: boolean;
    salaryRatio: SalaryRatio;

    constructor() {
    }

    static fromData(other: any): EmployeeType {
        let employeeType = new EmployeeType();
        employeeType.id = other.id || Util.getEmptyId();
        employeeType.value = other.value || "";
        employeeType.canHaveSubordinates = Boolean(other.canHaveSubordinates) || false;
        employeeType.salaryRatio =
            other.salaryRatio && SalaryRatio.fromData(other.salaryRatio) || undefined;
        return employeeType;
    }

    static fromDataList(other: any[]): EmployeeType[] {
        return other.map(
            (employeeType: any, _index: Number, _employeeTypes: any[]) =>
                EmployeeType.fromData(employeeType));
    }
}