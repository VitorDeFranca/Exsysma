import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Exsysma';
  navItems = [
    { name: 'Projects', route: '/projects', icon: 'folder' },
    { name: 'Rules', route: '/rules', icon: 'gavel' },
    { name: 'Variables', route: '/variables', icon: 'code' },
    { name: 'About', route: '/about', icon: 'info' }
  ];

  constructor(private router: Router) {}

  navigateTo(route: string): void {
    this.router.navigate([route]);
  }
}