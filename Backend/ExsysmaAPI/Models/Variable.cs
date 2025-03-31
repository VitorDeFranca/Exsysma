using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExsysmaAPI.Models
{
    public class Variable
    {
       public int Id { get; set; }
       public string Name { get; set; }
       public int ProjectId { get; set; }
       public Project Project { get; set; }
       public string? QuestionDescription { get; set; }
       public bool isGoalVariable { get; set; }
    }

    public class MultiValueVariable : Variable {
        public List<string> Values { get; set; }
    }

    public class SingleValueVariable : Variable {
        public string Value { get; set; }
    }
}