import {Component} from "@angular/core";
import {WorkerService} from "../../services/worker.service";
import {Worker} from "../../models/worker";
import {MatDialog} from "@angular/material/dialog";
import {EditWorkerDialogComponent} from "../edit-worker-dialog/edit.worker.dialog";
import {FormControl} from "@angular/forms";

@Component({
    selector: 'app-worker-list',
    templateUrl: "worker.list.html"
})
export class WorkerListComponent {
    dateControl: FormControl;
    workers: Worker[];

    constructor(
        private workerService: WorkerService,
        public dialog: MatDialog) {
        this.dateControl = new FormControl(new Date());
        this.workers = [];
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
        this.workerService.get(this.dateControl.value)
            .subscribe({
                next: (workers) => this.workers = workers,
                error: (error) => console.error(error)
            });
    }
}