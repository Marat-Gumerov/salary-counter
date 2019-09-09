import Util from "../util/util";
import WorkerType from "./worker.type";

export default class Worker {
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
}