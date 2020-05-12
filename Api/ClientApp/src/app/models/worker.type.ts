import {SalaryRatio} from "./salary.ratio";
import Util from "../util/util";

export default class WorkerType {
    id: string;
    value: string;
    canHaveSubordinates: boolean;
    salaryRatio: SalaryRatio;

    constructor() {
    }

    static fromData(other: any): WorkerType {
        let workerType = new WorkerType();
        workerType.id = other.id || Util.getEmptyId();
        workerType.value = other.value || "";
        workerType.canHaveSubordinates = Boolean(other.canHaveSubordinates) || false;
        workerType.salaryRatio = other.salaryRatio && SalaryRatio.fromData(other.salaryRatio) || undefined;
        return workerType;
    }

    static fromDataList(other: any[]): WorkerType[] {
        return other.map(function (workerType: any, index: Number, workerTypes: any[]) {
            return WorkerType.fromData(workerType);
        })
    }
}