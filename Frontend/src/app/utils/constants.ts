export class Constants {
    public static apiPort: string = 'http://localhost:5289';
}

export interface ColumnConfig {
  name: string;
  header: string;
  cell: (item: any) => any;
  sortable?: boolean;
}
