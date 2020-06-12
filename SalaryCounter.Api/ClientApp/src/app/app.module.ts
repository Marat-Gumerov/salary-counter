import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {BrowserModule} from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import {HashLocationStrategy, LocationStrategy} from '@angular/common';

import {MaterialModule} from './material.module';

import {ApplicationComponent} from './components/application/application';
import NavbarComponent from './components/navbar/navbar';
import HomeComponent from './components/home/home';
import SalaryComponent from './components/salary/salary';
import EmployeeTypeListComponent from './components/employee-type-list/employee.type.list';
import EmployeeTypeComponent from './components/employee-type/employee.type';

import {SalaryService} from './services/salary.service';
import {EmployeeTypeService} from './services/employee-type.service';
import {EmployeeListComponent} from './components/employee-list/employee.list';
import {EmployeeComponent} from './components/employee/employee';
import {EmployeeService} from './services/employee.service';
import {MatDialogModule} from '@angular/material/dialog';
import {EditEmployeeDialogComponent} from './components/edit-employee-dialog/edit.employee.dialog';
import {ReactiveFormsModule} from "@angular/forms";

const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'employeeTypes', component: EmployeeTypeListComponent},
    {path: 'employees', component: EmployeeListComponent}
]

@NgModule({
    imports: [
        BrowserModule,
        RouterModule.forRoot(routes),
        HttpClientModule,
        MaterialModule,
        MatDialogModule,
        ReactiveFormsModule
    ],
    declarations: [
        ApplicationComponent,
        NavbarComponent,
        HomeComponent,
        SalaryComponent,
        EmployeeTypeListComponent,
        EmployeeTypeComponent,
        EmployeeListComponent,
        EmployeeComponent,
        EditEmployeeDialogComponent
    ],
    entryComponents: [
        EditEmployeeDialogComponent
    ],
    providers: [
        SalaryService,
        EmployeeTypeService,
        EmployeeService,
        {
            provide: LocationStrategy,
            useClass: HashLocationStrategy
        }
    ],
    bootstrap: [ApplicationComponent]
})
export class AppModule {
}
