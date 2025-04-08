using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ExsysmaAPI.Models
{
    public class RuleItem
    {
        public int RuleItemId { get; set; }
        public int VariableId  { get; set; }
        public Variable Variable  { get; set; }
        public Operator Operator { get; set; }
        public string Value { get; set; }
    }

    public enum Operator {
        IsDifferent,
        Equals,
    }


}