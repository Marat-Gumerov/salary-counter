import { Component } from "@angular/core";
import { SalaryService } from "../../services/salary.service";

@Component({
    templateUrl: "salary.html",
    selector: 'app-salary'
})
export default class SalaryComponent {
    salary: number = -1;
    date: Date;
    constructor(private salaryService: SalaryService) {
        this.date = new Date();
        this.get();
    }
    get(): void
    {
        this.salaryService
            .get(this.date)
            .subscribe({
                next: (salary) => this.salary = salary,
                error: (error) => console.error(error)
            });
    }

    onDateChange(value: Date): void {
        this.date = new Date(value);
        this.get();
    }
}