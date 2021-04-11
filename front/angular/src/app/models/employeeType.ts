import {SalaryRatio} from "./salary.ratio";
import Util from "../util/util";

export class EmployeeType {

    constructor(
        public id: string,
        public value: string,
        public canHaveSubordinates: boolean,
        public salaryRatio: SalaryRatio
    ) {
    }

    static fromData(other: any): EmployeeType {
        return new EmployeeType(
            other.id || Util.getEmptyId(),
            other.value || "",
            Boolean(other.canHaveSubordinates) || false,
            other.salaryRatio && SalaryRatio.fromData(other.salaryRatio) || undefined,
        );
    }

    static fromDataList(other: any[]): EmployeeType[] {
        return other.map(employeeType => EmployeeType.fromData(employeeType));
    }
}
