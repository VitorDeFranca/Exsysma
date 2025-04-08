using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExsysmaAPI.Models.DTOs.Variable
{
    public class GetVariablesDTO
    {
       public string Name { get; set; }
       public string ProjectName { get; set; }

       public GetVariablesDTO()
       {

       }
    }
}