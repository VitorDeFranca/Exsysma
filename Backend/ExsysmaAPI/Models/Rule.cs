using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExsysmaAPI.Models;

public class Rule
{
    public int Id { get; set; }
    public List<RuleItem> Conditions { get; set; }
    public RuleItem Conclusion { get; set; }
    public int ProjectId { get; set; }

}