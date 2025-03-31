import { Project } from "./project";
import { Variable } from "./variable";

export class Rule {
    id: number;
    conditions: string[];
    goalVariableId: number;
    goalVariable: Variable;
    goalVariableValue: string;
    projectId: number;
    project: Project;

    getConclusion(): string {
        return `${this.goalVariable.name} = ${this.goalVariableValue}`;
      }

      getExplanation(): string {
        return `${this.goalVariable.name} = ${this.goalVariableValue}`;
      }
}
