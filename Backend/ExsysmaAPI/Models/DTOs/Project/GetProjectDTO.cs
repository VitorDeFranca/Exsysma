using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExsysmaAPI.Models.DTOs.Project
{
    public class GetProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Responsible { get; set; }
        public List<Rule>? Rules { get; set; }
        public List<Variable>? Variables { get; set; }
    }
}