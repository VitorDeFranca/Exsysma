using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ExsysmaAPI.Models.DTOs.Project;
using ExsysmaAPI.Models.DTOs.Rule;
using ExsysmaAPI.Models.DTOs.Variable;

namespace ExsysmaAPI.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public List<Rule>? Rules { get; set; }
    public List<Variable>? Variables { get; set; }

    public GetProjectsDTO ToProjectsDTO() {
        return new GetProjectsDTO {
            ProjectId = Id,
            Name = Name,
            Responsible = User.Username
        };
    }

    public GetProjectDTO ToProjectDTO() {
        return new GetProjectDTO {
            Name = Name,
            Responsible = User.Username,
            Rules = Rules is null
                ? new List<GetRulesDTO>()
                : Rules.Select(el => el.ToRulesDTO()).ToList(),
            Variables = Variables is null
                ? new List<GetVariablesDTO>()
                : Variables.Select(el => el.ToVariablesDTO()).ToList(),
        };
    }
}