using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExsysmaAPI.Models;

public class Rule
{
    public int Id { get; set; }
    public List<string> Conditions { get; set; }
    public int GoalVariableId { get ; set; }
    public Variable GoalVariable { get ; set; }
    public string GoalVariableValue { get ; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public string GetConclusion() => $"{GoalVariable.Name} = {GoalVariableValue}";
    public string GetExplanation() => $"{GoalVariable.Name} = {GoalVariableValue}";
}