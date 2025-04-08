using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExsysmaAPI.Models.DTOs.Rule
{
    public class GetRulesDTO
    {
        public GetRulesDTO() {}

        public int RuleId { get; set; }
        public List<string> Conditions { get; set; }
        public string Conclusion { get; set; }
    }
}