import {Component, Input} from "@angular/core";
import {SalaryService} from "../../services/salary.service";
import {Worker} from "../../models/worker";
import {MatDialog} from "@angular/material/dialog";
import {EditWorkerDialogComponent} from "../edit-worker-dialog/edit.worker.dialog";

@Component({
    templateUrl: "worker.html",
    selector: 'app-worker'
})
export class WorkerComponent {
    @Input() worker: Worker;
    @Input() date: Date;
    salary: number;

    constructor(
        private salaryService: SalaryService,
        public dialog: MatDialog) {
        this.salary = -1;
    }

    onGetSalaryClick(): void {
        this.salaryService.getById(this.date, this.worker.id)
            .subscribe({
                next: (salary) => this.salary = salary,
                error: (error) => console.error(error)
            });
    }

    onEdit(): void {
        const dialogRef = this.dialog.open(EditWorkerDialogComponent, {
            data: this.worker
        })
        dialogRef.afterClosed()
            .subscribe(result => this.worker = result);

    }
}