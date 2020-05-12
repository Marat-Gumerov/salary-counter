import Util from "../util/util";
import {WorkerType} from "./worker.type";

export class Worker {
    id: string;
    name: string;
    employmentDate: Date;
    salaryBase: number;
    workerType: WorkerType;
    chief: string;

    constructor() {
        this.id = Util.getEmptyId();
        this.name = "";
        this.salaryBase = 1000;
        this.employmentDate = new Date();
    }

    static fromData(other: any): Worker {
        let worker = new Worker();
        worker.id = other.id || worker.id;
        worker.name = other.name || worker.name;
        worker.salaryBase = other.salaryBase && Number(other.salaryBase) || worker.salaryBase;
        worker.employmentDate = other.employmentDate && new Date(other.employmentDate) || worker.employmentDate;
        worker.workerType = other.workerType && WorkerType.fromData(other.workerType) || undefined;
        worker.chief = other.chief || worker.chief;
        return worker;
    }

    static fromDataList(other: any[]): Worker[] {
        return other.map(function (worker: any, index: Number, workers: any[]) {
            return Worker.fromData(worker);
        })
    }
}