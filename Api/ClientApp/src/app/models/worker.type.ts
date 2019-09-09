import SalaryRatio from "./salary.ratio";

export default class WorkerType {
    id: string;
    value: string;
    canHaveSubordinates: boolean;
    salaryRatio: SalaryRatio;
}