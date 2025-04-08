using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExsysmaAPI.Models.DTOs.Rule;
using ExsysmaAPI.Models.DTOs.Variable;

namespace ExsysmaAPI.Models.DTOs.Project
{
    public class GetProjectDTO
    {
        public GetProjectDTO() {}

        public string Name { get; set; }
        public string Responsible { get; set; }
        public List<GetRulesDTO>? Rules { get; set; }
        public List<GetVariablesDTO>? Variables { get; set; }
    }
}