import { Project } from "./project";

export class Variable {
    id: number;
    name: string;
    projectId: number;
    project: Project;
    questionDescription?: string;
    isGoalVariable: boolean;
}
