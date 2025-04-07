using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ExsysmaAPI.Models
{
    public class RuleItem
    {
        public int VariableId  { get; set; }
        public Variable Variable { get; set; }
        public Operator Operator { get; set; }
        public string Value { get; set; }
    }

    public enum Operator {
        Equals,
        IsDifferent,
    }


}