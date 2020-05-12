import {Component, Input} from "@angular/core";
import {WorkerType} from '../../models/worker.type';

@Component({
    templateUrl: "worker.type.html",
    selector: 'app-worker-type'
})
export default class WorkerTypeComponent {
    @Input() workerType: WorkerType;
}