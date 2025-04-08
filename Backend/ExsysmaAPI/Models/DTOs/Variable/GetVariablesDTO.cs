using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExsysmaAPI.Models.DTOs.Variable
{
    public class GetVariablesDTO
    {
        public int VariableId { get; set; }
        public string Name { get; set; }
        public bool IsGoalVariable { get; set; }

        public GetVariablesDTO()
        {

        }
    }
}