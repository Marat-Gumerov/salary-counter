import { Component, Input } from "@angular/core";
import { SalaryService } from "../../services/salary.service";
import Worker from "../../models/worker";

@Component({
    templateUrl: "worker.html",
    selector: 'app-worker'
})
export default class WorkerComponent {
    @Input() worker: Worker;
    @Input() date: Date;
    salary: number;
    constructor(private salaryService: SalaryService) {
        this.salary = -1;
    }

    onGetSalaryClick(id: string): void {
        this.salaryService.getById(this.date, this.worker.id)
            .subscribe({
                next: (salary) => this.salary = salary,
                error: (error) => console.error(error)
            });
    }
}