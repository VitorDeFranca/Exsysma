using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExsysmaAPI.Models.DTOs.Variable;

namespace ExsysmaAPI.Models
{
    public class Variable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string? QuestionDescription { get; set; }
        public bool IsGoalVariable { get; set; }
        public List<string> PossibleValues { get; set; }
        public VariableType Type { get; set; }

        public GetVariablesDTO ToGetVariablesDTO(Variable variable)
        {
            return new GetVariablesDTO
            {
                Name = variable.Name,
                ProjectName = variable.Project.Name
            };
        }
    }

    public enum VariableType
    {
        SingleValue,
        MultiValue
    }


}