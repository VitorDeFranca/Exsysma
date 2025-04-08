using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExsysmaAPI.Models.DTOs.Project
{
    public class GetProjectsDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Responsible { get; set; }

        public GetProjectsDTO() {}
    }
}