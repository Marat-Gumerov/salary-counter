import { Component } from "@angular/core";
import { WorkerService } from "../../services/worker.service";
import Worker from "../../models/worker";
import { MatDialog } from "@angular/material";
import { EditWorkerDialogComponent } from "../edit-worker-dialog/edit.worker.dialog";

@Component({
    templateUrl: "worker.list.html",
    selector: 'app-worker-list'
})
export default class WorkerListComponent {
    date: Date;
    workers: Worker[];

    constructor(
        private workerService: WorkerService,
        public dialog: MatDialog) {
        this.date = new Date();
        this.workers = [];
        this.get();
    }

    onDateChange(value: Date): void {
        this.date = new Date(value);
        this.get();
    }

    add(): void {
        const dialogRef = this.dialog.open(EditWorkerDialogComponent, {
            data: new Worker()
        })
        dialogRef.afterClosed()
            .subscribe(result => this.get());
    }

    get(): void {
        this.workerService.get(this.date)
            .subscribe({
                next: (workers) => this.workers = workers,
                error: (error) => console.error(error)
            });
    }
}