import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ColumnConfig } from 'src/app/utils/constants';

@Component({
  selector: 'app-datatable',
  templateUrl: './datatable.component.html',
  styleUrls: ['./datatable.component.scss']
})
export class DataTableComponent implements OnInit {
  @Input() data: any[] = [];
  @Input() columns: ColumnConfig[] = [];
  @Input() showActions: boolean = true;
  @Input() title: string = '';
  @Input() noDataMessage: string = 'No data found.';
  @Input() addButtonLabel: string = 'Add New';

  @Output() add = new EventEmitter<void>();
  @Output() edit = new EventEmitter<any>();
  @Output() delete = new EventEmitter<any>();

  displayedColumns: string[] = [];

  ngOnInit(): void {
    this.displayedColumns = this.columns.map(col => col.name);
    if (this.showActions) {
      this.displayedColumns.push('actions');
    }
  }

  onAdd(): void {
    this.add.emit();
  }

  onEdit(item: any): void {
    this.edit.emit(item);
  }

  onDelete(item: any): void {
    this.delete.emit(item);
  }
}
