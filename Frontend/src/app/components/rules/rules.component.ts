import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Rule } from '../../models/rule';
import { Project } from '../../models/project';
import { RuleService } from '../../services/rule.service';
import { ProjectService } from '../../services/project.service';
// import { RuleDialogComponent } from './rule-dialog/rule-dialog.component';

@Component({
  selector: 'app-rules',
  templateUrl: './rules.component.html',
  styleUrls: ['./rules.component.scss']
})
export class RulesComponent implements OnInit {
  rules: Rule[] = [];
  projects: Project[] = [];
  selectedProjectId: number | null = null;
  displayedColumns: string[] = ['name', 'description', 'condition', 'action', 'priority', 'isActive', 'actions'];

  constructor(
    private ruleService: RuleService,
    private projectService: ProjectService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadProjects();
    this.loadRules();
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe(
      projects => this.projects = projects,
      error => this.showError('Failed to load projects')
    );
  }

  loadRules(): void {
    this.ruleService.getRules(this.selectedProjectId || undefined).subscribe(
      rules => this.rules = rules,
      error => this.showError('Failed to load rules')
    );
  }

  onProjectChange(): void {
    this.loadRules();
  }

  openRuleDialog(rule?: Rule): void {
    // const dialogRef = this.dialog.open(RuleDialogComponent, {
    //   width: '600px',
    //   data: {
    //     rule: rule || { projectId: this.selectedProjectId || undefined, isActive: true },
    //     projects: this.projects
    //   }
    // });

    // dialogRef.afterClosed().subscribe(result => {
    //   if (result) {
    //     if (result.id) {
    //       this.updateRule(result);
    //     } else {
    //       this.createRule(result);
    //     }
    //   }
    // });
  }

  createRule(rule: Partial<Rule>): void {
    this.ruleService.createRule(rule).subscribe(
      newRule => {
        this.rules.push(newRule);
        this.showSuccess('Rule created successfully');
      },
      error => this.showError('Failed to create rule')
    );
  }

  updateRule(rule: Rule): void {
    this.ruleService.updateRule(rule.id, rule).subscribe(
      updatedRule => {
        const index = this.rules.findIndex(r => r.id === updatedRule.id);
        if (index !== -1) {
          this.rules[index] = updatedRule;
        }
        this.showSuccess('Rule updated successfully');
      },
      error => this.showError('Failed to update rule')
    );
  }

  deleteRule(rule: Rule): void {
    // if (confirm(`Are you sure you want to delete the rule "${rule.name}"?`)) {
    //   this.ruleService.deleteRule(rule.id).subscribe(
    //     () => {
    //       this.rules = this.rules.filter(r => r.id !== rule.id);
    //       this.showSuccess('Rule deleted successfully');
    //     },
    //     error => this.showError('Failed to delete rule')
    //   );
    // }
  }

  showSuccess(message: string): void {
    this.snackBar.open(message, 'Close', { duration: 3000 });
  }

  showError(message: string): void {
    this.snackBar.open(message, 'Close', { duration: 5000, panelClass: ['error-snackbar'] });
  }
}