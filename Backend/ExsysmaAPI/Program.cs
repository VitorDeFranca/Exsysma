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

// ==================== PROJECT ENDPOINTS ====================

// GET: Listar todos os projetos
app.MapGet("/projects", async (AppDbContext db) =>
    await db.Projects.ToListAsync());

// GET: Obter um projeto específico por ID
app.MapGet("/projects/{id}", async (int id, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(id);
    return project != null ? Results.Ok(project) : Results.NotFound();
});

// POST: Criar um novo projeto
app.MapPost("/projects", async (Project project, AppDbContext db) =>
{
    db.Projects.Add(project);
    await db.SaveChangesAsync();
    return Results.Created($"/projects/{project.Id}", project);
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
    return Results.Ok(await db.Projects.FindAsync(id));
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

// ==================== VARIABLE ENDPOINTS ====================

// GET: Listar todas as variáveis de um projeto
app.MapGet("/projects/{projectId}/variables", async (int projectId, AppDbContext db) =>
{
    var variables = await db.Variables
        .Where(v => v.ProjectId == projectId)
        .ToListAsync();
    return Results.Ok(variables);
});

// GET: Obter uma variável específica
app.MapGet("/projects/{projectId}/variables/{id}", async (int projectId, int id, AppDbContext db) =>
{
    var variable = await db.Variables
        .FirstOrDefaultAsync(v => v.ProjectId == projectId && v.Id == id);
    return variable != null ? Results.Ok(variable) : Results.NotFound();
});

// POST: Criar uma nova variável
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

// PUT: Atualizar uma variável existente
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

// DELETE: Excluir uma variável
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

// ==================== RULE ENDPOINTS ====================

// GET: Listar todas as regras de um projeto
app.MapGet("/projects/{projectId}/rules", async (int projectId, AppDbContext db) =>
{
    var rules = await db.Rules
        .Where(r => r.ProjectId == projectId)
        .ToListAsync();
    return Results.Ok(rules);
});

//GET: Obter uma regra específica com suas condições e conclusões
app.MapGet("/projects/{projectId}/rules/{id}", async (int projectId, int id, AppDbContext db) =>
{
   var rule = await db.Rules.FirstOrDefaultAsync(r => r.ProjectId == projectId && r.Id == id);

   return rule != null ? Results.Ok(rule) : Results.NotFound();
});

// POST: Criar uma nova regra
app.MapPost("/projects/{projectId}/rules", async (int projectId, Rule rule, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(projectId);
    if (project == null)
        return Results.NotFound("Projeto não encontrado");

    rule.ProjectId = projectId;
    db.Rules.Add(rule);
    await db.SaveChangesAsync();
    return Results.Created($"/projects/{projectId}/rules/{rule.Id}", rule);
});

// PUT: Atualizar uma regra existente
app.MapPut("/projects/{projectId}/rules/{id}", async (int projectId, int id, Rule ruleInput, AppDbContext db) =>
{
    var rule = await db.Rules.FirstOrDefaultAsync(r => r.ProjectId == projectId && r.Id == id);
    if (rule == null)
        return Results.NotFound();

    rule.GoalVariable = ruleInput.GoalVariable;
    rule.GoalVariableValue = ruleInput.GoalVariableValue;
    rule.Conditions = ruleInput.Conditions;

    await db.SaveChangesAsync();
    return Results.Ok(await db.Rules.FindAsync(id));
});

// DELETE: Excluir uma regra
app.MapDelete("/projects/{projectId}/rules/{id}", async (int projectId, int id, AppDbContext db) =>
{
    var rule = await db.Rules.FirstOrDefaultAsync(r => r.ProjectId == projectId && r.Id == id);
    if (rule == null)
        return Results.NotFound();

    db.Rules.Remove(rule);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.UseHttpsRedirection();
app.Run();