import {Component} from "@angular/core";
import {SalaryService} from "../../services/salary.service";
import {FormControl} from "@angular/forms";

@Component({
    templateUrl: "salary.html",
    selector: 'app-salary'
})
export default class SalaryComponent {
    salary: number = -1;
    dateControl: FormControl;

    constructor(private salaryService: SalaryService) {
        this.dateControl = new FormControl(new Date);
        this.get();
    }

    get(): void {
        this.salaryService
            .get(this.dateControl.value)
            .subscribe({
                next: (salary) => this.salary = salary,
                error: (error) => console.error(error)
            });
    }
}