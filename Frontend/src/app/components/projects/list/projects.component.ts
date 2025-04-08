import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Project } from '../../../models/project';
import { ProjectService } from '../../../services/project.service';
import { DatePipe } from '@angular/common';
import { ColumnConfig } from 'src/app/utils/constants';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EntityModalComponent, FieldConfig } from 'src/app/shared/entity-modal/entity-modal.component';
import { Validators } from '@angular/forms';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss'],
  providers: [DatePipe]
})
export class ProjectsComponent implements OnInit {
  projects: Project[] = [];

  tableColumns: ColumnConfig[] = [
    {
      name: 'id',
      header: 'ID',
      cell: (project: Project) => project.id
    },
    {
      name: 'name',
      header: 'Name',
      cell: (project: Project) => project.name
    },
    {
      name: 'responsible',
      header: 'Responsible',
      cell: (project: Project) => project.responsible
    },
  ];

  constructor(
    private projectService: ProjectService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private datePipe: DatePipe,
  ) { }

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe(
      projects => this.projects = projects,
      error => this.showError('Failed to load projects')
    );
  }

  onAddProject(): void {
    const projectFields: FieldConfig[] = [
        {
          key: 'name',
          label: 'Project Name',
          type: 'text',
          required: true,
          hint: 'Enter a unique project name'
        },
        {
          key: 'responsible',
          label: 'Responsble',
          type: 'text',
          required: true,
          hint: 'Responsible for the project'
        }
    ]

    console.log(projectFields);
    const dialogRef = this.dialog.open(EntityModalComponent, {
        width: '600px',
        data: {
          title: 'Create Project',
          icon: 'assessment',
          fields: projectFields,
          submitButtonText: 'Create Project'
        }
      });

      // Get data when dialog closes
      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          console.log('New project data:', result);
          // Handle the submitted data (e.g., save to database)
        }
      });
  }

  onEditProject(project: Project): void {
    this.openProjectDialog(project);
  }

  onDeleteProject(project: Project): void {
    this.deleteProject(project);
  }

  openProjectDialog(project?: Project): void {
    // Dialog code would go here, commenting out as in your example
    // const dialogRef = this.dialog.open(ProjectDialogComponent, {...});
    // dialogRef.afterClosed().subscribe(...);
  }

  createProject(project: Partial<Project>): void {
    this.projectService.createProject(project).subscribe(
      newProject => {
        this.projects.push(newProject);
        this.showSuccess('Project created successfully');
      },
      error => this.showError('Failed to create project')
    );
  }

  updateProject(project: Project): void {
    this.projectService.updateProject(project.id, project).subscribe(
      updatedProject => {
        const index = this.projects.findIndex(p => p.id === updatedProject.id);
        if (index !== -1) {
          this.projects[index] = updatedProject;
        }
        this.showSuccess('Project updated successfully');
      },
      error => this.showError('Failed to update project')
    );
  }

  deleteProject(project: Project): void {
    if (confirm(`Are you sure you want to delete the project "${project.name}"?`)) {
      this.projectService.deleteProject(project.id).subscribe(
        () => {
          this.projects = this.projects.filter(p => p.id !== project.id);
          this.showSuccess('Project deleted successfully');
        },
        error => this.showError('Failed to delete project')
      );
    }
  }

  showSuccess(message: string): void {
    this.snackBar.open(message, 'Close', { duration: 3000 });
  }

  showError(message: string): void {
    this.snackBar.open(message, 'Close', { duration: 5000, panelClass: ['error-snackbar'] });
  }


}