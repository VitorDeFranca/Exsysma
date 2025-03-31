import { Rule } from "./rule";
import { Variable } from "./variable";

export class Project {
    id: number;
    name: string;
    responsible: string;
    rules: Rule[];
    variables: Variable[];
}
