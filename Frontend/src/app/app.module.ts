// app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

// Angular Material Imports
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';

// Components
import { AppComponent } from './app.component';
import { ProjectsComponent } from './components/projects/list/projects.component';
import { RulesComponent } from './components/rules/rules.component';
import { VariablesComponent } from './components/variables/variables.component';
import { AboutComponent } from './components/about/about.component';
// import { ProjectDialogComponent } from './components/projects/project-dialog/project-dialog.component';
// import { RuleDialogComponent } from './components/rules/rule-dialog/rule-dialog.component';
// import { VariableDialogComponent } from './components/variables/variable-dialog/variable-dialog.component';

// Services
import { ProjectService } from './services/project.service';
import { RuleService } from './services/rule.service';
import { VariableService } from './services/variable.service';
import { DataTableComponent } from './shared/datatable/datatable.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EntityModalComponent } from './shared/entity-modal/entity-modal.component';

const routes: Routes = [
  { path: 'projects', component: ProjectsComponent },
  { path: 'rules', component: RulesComponent },
  { path: 'variables', component: VariablesComponent },
  { path: 'about', component: AboutComponent },
  { path: '', redirectTo: '/projects', pathMatch: 'full' }
];

@NgModule({
  declarations: [
    AppComponent,
    ProjectsComponent,
    RulesComponent,
    VariablesComponent,
    AboutComponent,
    DataTableComponent,
    EntityModalComponent
    // ProjectDialogComponent,
    // RuleDialogComponent,
    // VariableDialogComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes),
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatTabsModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDialogModule,
    MatSnackBarModule,
    NgbModule,
    ReactiveFormsModule
  ],
  providers: [
    ProjectService,
    RuleService,
    VariableService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }