using ExsysmaAPI;
using ExsysmaAPI.Data;
using ExsysmaAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=exsysma.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region projects
app.MapGet("/projects", async (AppDbContext db) =>
{
    var projects = db.Projects;

    var projectsDTO = await projects
        .Select(el => el.ToProjectsDTO())
        .ToListAsync();

    return Results.Ok(projectsDTO);
});

app.MapGet("/projects/{id}", async (int id, AppDbContext db) =>
{
    var project = db.Projects
        .Where(el => el.Id == id)
        .Include(el => el.Rules!)
            .ThenInclude(el => el.Conditions)
            .ThenInclude(el => el.Variable)
        .Include(el => el.Rules!)
            .ThenInclude(el => el.Conclusion)
            .ThenInclude(el => el.Variable)
        .Include(el => el.Variables);

    var projectDTO = await project
        .Select(el => el.ToProjectDTO())
        .FirstOrDefaultAsync();

    return project != null ? Results.Ok(projectDTO) : Results.NotFound();
});

// POST: Criar um novo projeto
app.MapPost("/projects", async (Project project, AppDbContext db) =>
{
    db.Projects.Add(project);
    await db.SaveChangesAsync();
    return Results.Created($"/projects/{project.Id}", project.ToProjectDTO());
});

// PUT: Atualizar um projeto existente
app.MapPut("/projects/{id}", async (int id, Project projectInput, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
        return Results.NotFound();

    project.Name = projectInput.Name;
    project.Responsible = projectInput.Responsible;

    await db.SaveChangesAsync();
    var newProject = await db.Projects.FindAsync(id)
    return Results.Ok(newProject.ToProjectDTO());
});

// DELETE: Excluir um projeto
app.MapDelete("/projects/{id}", async (int id, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
        return Results.NotFound();

    db.Projects.Remove(project);
    await db.SaveChangesAsync();
    return Results.Ok();
});
#endregion

#region variables
app.MapGet("/projects/{projectId}/variables", async (int projectId, AppDbContext db) =>
{
    var variables = db.Variables
        .Where(v => v.ProjectId == projectId);

    var variablesDTO = await variables
        .Select(el => el.ToVariablesDTO())
        .ToListAsync();

    return Results.Ok(variablesDTO);
});

app.MapGet("/projects/{projectId}/variables/{id}", async (int projectId, int id, AppDbContext db) =>
{
    var variable = await db.Variables
        .FirstOrDefaultAsync(v => v.ProjectId == projectId && v.Id == id);
    return variable != null ? Results.Ok(variable) : Results.NotFound();
});

app.MapPost("/projects/{projectId}/variables", async (int projectId, Variable variable, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(projectId);
    if (project == null)
        return Results.NotFound("Projeto não encontrado");

    variable.ProjectId = projectId;
    if (variable.Type == VariableType.SingleValue && variable.PossibleValues.Count > 1)
        return Results.BadRequest("A variável não pode possuir múltiplos valores se ela é univalorada");
    db.Variables.Add(variable);
    await db.SaveChangesAsync();
    return Results.Created($"/projects/{projectId}/variables/{variable.Id}", variable);
});

app.MapPut("/projects/{projectId}/variables/{id}", async (int projectId, int id, Variable variableInput, AppDbContext db) =>
{
    var variable = await db.Variables
        .FirstOrDefaultAsync(v => v.ProjectId == projectId && v.Id == id);
    if (variable == null)
        return Results.NotFound();

    variable.Name = variableInput.Name;
    variable.IsGoalVariable = variableInput.IsGoalVariable;
    variable.QuestionDescription = variableInput.QuestionDescription;
    variable.Type = variableInput.Type;
    variable.PossibleValues = variableInput.PossibleValues;

    if (variable.Type == VariableType.SingleValue && variable.PossibleValues.Count > 1)
        return Results.BadRequest("A variável não pode possuir múltiplos valores se ela é univalorada");

    await db.SaveChangesAsync();
    return Results.Ok(await db.Variables.FindAsync(id));
});

app.MapDelete("/projects/{projectId}/variables/{id}", async (int projectId, int id, AppDbContext db) =>
{
    var variable = await db.Variables
        .FirstOrDefaultAsync(v => v.ProjectId == projectId && v.Id == id);
    if (variable == null)
        return Results.NotFound();

    db.Variables.Remove(variable);
    await db.SaveChangesAsync();
    return Results.Ok();
});
#endregion

#region rules
app.MapGet("/projects/{projectId}/rules", async (int projectId, AppDbContext db) =>
{
    var rules = db.Rules
        .Where(r => r.ProjectId == projectId)
        .Include(el => el.Conclusion)
            .ThenInclude(c => c.Variable)
        .Include(el => el.Conditions)
            .ThenInclude(c => c.Variable);

    var rulesDTO = await rules
        .Select(el => el.ToRulesDTO())
        .ToListAsync();

    return Results.Ok(rulesDTO);
});

app.MapGet("/projects/{projectId}/rules/{id}", async (int projectId, int id, AppDbContext db) =>
{
    var rule = await db.Rules
        .Include(r => r.Conclusion)
            .ThenInclude(c => c.Variable)
        .Include(r => r.Conditions)
            .ThenInclude(condition => condition.Variable)
        .FirstOrDefaultAsync(r => r.ProjectId == projectId && r.Id == id);

    if (rule is null) return Results.NotFound();

    var readableRule = Utils.ConvertToReadableRule(rule);

    return Results.Ok(readableRule);
});

app.MapPost("/projects/{projectId}/rules", async (int projectId, Rule rule, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(projectId);
    if (project == null)
        return Results.NotFound("Projeto não encontrado");

    rule.ProjectId = projectId;

    //verifica se as variáveis de condição existem naquele projeto e se o valor delas está dentro do esperado
    foreach (var condition in rule.Conditions) {
        var variableId = condition.VariableId;
        if (!db.Variables.Any(el => el.Id == variableId && el.ProjectId == projectId))
            return Results.BadRequest($"Não existe variável de ID {condition.VariableId} para o projeto de ID {projectId}");

        var variable = await db.Variables.FindAsync(variableId);
        if (!variable.PossibleValues.Contains(condition.Value))
            return Results.BadRequest($"Não existe o valor {condition.Value} para a variável de ID {condition.VariableId}");
    }

    //verfica se a variável de conclusão existe nesse projeto, se ela é variável-objetivo e se o valor dela está dentro do esperado
    if (!db.Variables.Any(el => el.Id == rule.Conclusion.VariableId && el.ProjectId == projectId))
        return Results.BadRequest($"Não existe variável de ID {rule.Conclusion.VariableId} para o projeto de ID {projectId}");
    var conclusionVariable = await db.Variables.FindAsync(rule.Conclusion.VariableId);
    if (!conclusionVariable.IsGoalVariable)
        return Results.BadRequest("Uma variável que não seja objetivo não pode ser usada na conclusão.");
    if (!conclusionVariable.PossibleValues.Contains(rule.Conclusion.Value))
        return Results.BadRequest($"Não existe o valor {rule.Conclusion.Value} para a variável de ID {conclusionVariable.Id}");

    db.Rules.Add(rule);
    await db.SaveChangesAsync();
    return Results.Created($"/projects/{projectId}/rules/{rule.Id}", rule.ToRulesDTO());
});

app.MapPut("/projects/{projectId}/rules/{id}", async (int projectId, int id, Rule ruleInput, AppDbContext db) =>
{
    var rule = await db.Rules.FirstOrDefaultAsync(r => r.ProjectId == projectId && r.Id == id);
    if (rule == null)
        return Results.NotFound();

    rule.Conditions = ruleInput.Conditions;
    rule.Conclusion = ruleInput.Conclusion;

    if (rule.ProjectId != ruleInput.ProjectId)
        return Results.BadRequest("Uma regra não pode passar para outro projeto");

    await db.SaveChangesAsync();
    var newRule = await db.Rules.FindAsync(id);
    return Results.Ok(newRule.ToRulesDTO);
});

app.MapDelete("/projects/{projectId}/rules/{id}", async (int projectId, int id, AppDbContext db) =>
{
    var rule = await db.Rules.FirstOrDefaultAsync(r => r.ProjectId == projectId && r.Id == id);
    if (rule == null)
        return Results.NotFound();

    db.Rules.Remove(rule);
    await db.SaveChangesAsync();
    return Results.Ok();
});
#endregion

app.UseHttpsRedirection();
app.Run();