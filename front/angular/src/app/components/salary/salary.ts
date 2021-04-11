import {Component} from "@angular/core";
import {SalaryService} from "../../services/salary.service";
import {FormControl} from "@angular/forms";

@Component({
    templateUrl: "salary.html",
    selector: 'app-salary'
})
export class SalaryComponent {
    salary: number = -1;
    dateControl: FormControl;

    constructor(private salaryService: SalaryService) {
        this.dateControl = new FormControl(new Date);
        this.get();
    }

    async get(): Promise<void> {
        this.salary = await this.salaryService
            .get(this.dateControl.value);
    }
}