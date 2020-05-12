import {Component} from "@angular/core";
import {WorkerTypeService} from "../../services/worker.type.service";
import WorkerType from "../../models/worker.type";

@Component({
    templateUrl: "worker.type.list.html",
    selector: 'app-worker-type-list'
})
export default class WorkerTypeListComponent {
    workerTypes: WorkerType[];

    constructor(private workerTypeService: WorkerTypeService) {
        this.workerTypes = [];
        this.get();
    }

    get(): void {
        this.workerTypeService.get()
            .subscribe({
                next: (workerTypes) => this.workerTypes = workerTypes,
                error: (error) => console.error(error)
            });
    }
}