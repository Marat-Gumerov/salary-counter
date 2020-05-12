import {Component} from "@angular/core";
import {WorkerService} from "../../services/worker.service";
import Worker from "../../models/worker";
import {MatDialog} from "@angular/material/dialog";
import {EditWorkerDialogComponent} from "../edit-worker-dialog/edit.worker.dialog";

@Component({
    selector: 'app-worker-list',
    templateUrl: "worker.list.html"
})
export class WorkerListComponent {
    date: Date;
    workers: Worker[];

    constructor(
        private workerService: WorkerService,
        public dialog: MatDialog) {
        this.date = new Date();
        this.workers = [];
        this.get();
    }

    onDateChange(value: string): void {
        this.date = new Date(value);
        this.get();
    }

    add(): void {
        const dialogRef = this.dialog.open(EditWorkerDialogComponent, {
            data: new Worker()
        })
        dialogRef.afterClosed()
            .subscribe(() => this.get());
    }

    get(): void {
        this.workerService.get(this.date)
            .subscribe({
                next: (workers) => this.workers = Worker.fromDataList(workers),
                error: (error) => console.error(error)
            });
    }
}