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
import WorkerTypeListComponent from './components/worker-type-list/worker.type.list';
import WorkerTypeComponent from './components/worker-type/worker.type';

import {SalaryService} from './services/salary.service';
import {WorkerTypeService} from './services/worker.type.service';
import {WorkerListComponent} from './components/worker-list/worker.list';
import {WorkerComponent} from './components/worker/worker';
import {WorkerService} from './services/worker.service';
import {MatDialogModule} from '@angular/material/dialog';
import {EditWorkerDialogComponent} from './components/edit-worker-dialog/edit.worker.dialog';
import {ReactiveFormsModule} from "@angular/forms";

const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'workerTypes', component: WorkerTypeListComponent},
    {path: 'workers', component: WorkerListComponent}
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
        WorkerTypeListComponent,
        WorkerTypeComponent,
        WorkerListComponent,
        WorkerComponent,
        EditWorkerDialogComponent
    ],
    entryComponents: [
        EditWorkerDialogComponent
    ],
    providers: [
        SalaryService,
        WorkerTypeService,
        WorkerService,
        {
            provide: LocationStrategy,
            useClass: HashLocationStrategy
        }
    ],
    bootstrap: [ApplicationComponent]
})
export class AppModule {
}
