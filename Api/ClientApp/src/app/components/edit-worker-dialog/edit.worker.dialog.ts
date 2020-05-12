import {Component, Inject} from "@angular/core";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Worker} from "../../models/worker";
import Util from "../../util/util";
import {WorkerService} from "../../services/worker.service";
import {Observable} from "rxjs";
import {WorkerTypeService} from "../../services/worker.type.service";
import {WorkerType} from "../../models/worker.type";
import {FormControl} from "@angular/forms";

@Component({
    selector: 'app-edit-worker-dialog',
    templateUrl: 'edit.worker.dialog.html'
})
export class EditWorkerDialogComponent {
    state: string;
    workerTypes: WorkerType[];
    chiefs: Worker[];
    employmentDateControl: FormControl;
    workerTypeControl: FormControl;
    chiefControl: FormControl;

    constructor(
        public dialogRef: MatDialogRef<EditWorkerDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public worker: Worker,
        private workerService: WorkerService,
        private workerTypeService: WorkerTypeService) {
        this.employmentDateControl = new FormControl(worker.employmentDate);
        this.workerTypeControl = new FormControl(worker.workerType?.id);
        this.chiefControl = new FormControl(worker.chief);
        if (worker.id === Util.getEmptyId()) {
            this.state = 'Add';
        } else {
            this.state = 'Edit';
        }
        this.getWorkerTypes();
        this.getChiefs();
    }

    onNameInput(value: string): void {
        this.worker.name = value;
    }

    onEmploymentDateChange(date: string): void {
        this.worker.employmentDate = new Date(date);
        this.getChiefs();
    }

    onSalaryBaseInput(value: string): void {
        this.worker.salaryBase = Number(value);
    }

    onSaveClick(): void {
        let observable: Observable<Worker>;
        if (this.state === 'Add') {
            observable = this.workerService.add(this.worker);
        } else {
            observable = this.workerService.update(this.worker);
        }
        observable.subscribe({
            next: (worker) => {
                this.worker = worker;
                this.dialogRef.close(worker);
            },
            error: (error) => console.error(error)
        });
    }

    onCancelClick(): void {
        this.dialogRef.close();
    }

    onWorkerTypeChange(value: string): void {
        this.worker.workerType = this.workerTypes.find(workerType => workerType.id === value);
    }

    onChiefChange(value: string): void {
        this.worker.chief = value.length > 0 ? value : undefined;
    }

    private getWorkerTypes(): void {
        this.workerTypeService.get()
            .subscribe({
                next: (workerTypes) => {
                    this.workerTypes = WorkerType.fromDataList(workerTypes);
                    this.worker.workerType = this.worker.workerType
                        ? this.workerTypes.find(workerType => workerType.id === this.worker.workerType.id)
                        : this.workerTypes[0];
                    this.workerTypeControl.setValue(this.worker?.workerType?.id);
                },
                error: (error) => console.error(error)
            });
    }

    private getChiefs(): void {
        this.workerService.get(this.worker.employmentDate)
            .subscribe({
                next: (workers) => {
                    this.chiefs = workers;
                    this.chiefs.unshift(undefined);
                    this.chiefControl.setValue(this.worker?.chief);
                },
                error: (error) => console.error(error)
            });
    }
}