using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExsysmaAPI.Models.DTOs.Rule;

namespace ExsysmaAPI.Models;

public class Rule
{
    public int Id { get; set; }
    public List<RuleItem> Conditions { get; set; }
    public RuleItem Conclusion { get; set; }
    public int ProjectId { get; set; }

    public GetRulesDTO ToRulesDTO() {
        return new GetRulesDTO {
            RuleId = Id,
            Conditions = Conditions.Select(el => Utils.ConvertToReadableRuleItem(el)).ToList(),
            Conclusion = Utils.ConvertToReadableRuleItem(Conclusion)
        };
    }

}