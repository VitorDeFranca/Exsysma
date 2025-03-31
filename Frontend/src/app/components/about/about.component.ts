// About Component
// components/about/about.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss']
})
export class AboutComponent {
  appName = 'Expert System Management UI';
  version = '1.0.0';
  description = 'A management interface for expert systems, allowing creation and maintenance of projects, rules, and variables.';

  features = [
    'Project Management: Create, read, update, and delete projects',
    'Rule Management: Define conditions and actions for your expert system',
    'Variable Management: Maintain the variables used in your system',
    'Responsive UI: Works on desktop and mobile devices'
  ];

  technologies = [
    'Angular 16',
    'Angular Material',
    'TypeScript',
    'RxJS'
  ];
}