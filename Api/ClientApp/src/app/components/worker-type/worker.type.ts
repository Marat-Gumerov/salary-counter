import { Component, Input } from "@angular/core";

@Component({
    templateUrl: "worker.type.html",
    selector: 'app-worker-type'
})
export default class WorkerTypeComponent {
    @Input() workerType: WorkerType;
}