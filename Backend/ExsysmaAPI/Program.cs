using System.Text;
using ExsysmaAPI;
using ExsysmaAPI.Data;
using ExsysmaAPI.Models;
using ExsysmaAPI.Models.DTOs.User;
using ExsysmaAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(Secrets.JwtSecret);
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("User", policy => policy.RequireRole("user"));
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=exsysma.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region users
app.MapPost("/register", async (User user, AppDbContext db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
}).AllowAnonymous();

app.MapPost("/login", async (UserLogin loginUser, AppDbContext db) =>
{
    var user = await db.Users
        .FirstOrDefaultAsync(el => el.Username == loginUser.Username);

    if (user == null)
        return Results.BadRequest("User does not exist.");
    if (user.Password != loginUser.Password)
        return Results.BadRequest("Invalid credentials.");

    var token = TokenService.GenerateToken(user);
    user.Password = "";

    return Results.Ok(new {user, token});
}).AllowAnonymous();
#endregion

#region projects
app.MapGet("/projects", async (AppDbContext db) =>
{
    var projects = db.Projects;

    var projectsDTO = await projects
        .Include(el => el.User)
        .Select(el => el.ToProjectsDTO())
        .ToListAsync();

    return Results.Ok(projectsDTO);
}).RequireAuthorization();

app.MapGet("/projects/{id}", async (int id, AppDbContext db) =>
{
    var project = db.Projects
        .Where(el => el.Id == id)
        .Include(el => el.User)
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
}).RequireAuthorization();

app.MapPost("/projects", async (Project project, AppDbContext db) =>
{
    db.Projects.Add(project);
    await db.SaveChangesAsync();

    var newProject = await db.Projects
        .Where(el => el.Id == project.Id)
        .Include(el => el.User)
        .Include(el => el.Rules!)
            .ThenInclude(el => el.Conditions)
            .ThenInclude(el => el.Variable)
        .Include(el => el.Rules!)
            .ThenInclude(el => el.Conclusion)
            .ThenInclude(el => el.Variable)
        .Include(el => el.Variables)
        .FirstOrDefaultAsync();

    return Results.Created($"/projects/{project.Id}", newProject.ToProjectDTO());
}).RequireAuthorization("Admin");

app.MapPut("/projects/{id}", async (int id, Project projectInput, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
        return Results.NotFound();

    project.Name = projectInput.Name;

    await db.SaveChangesAsync();

    var newProject = await db.Projects
        .Where(el => el.Id == project.Id)
        .Include(el => el.User)
        .Include(el => el.Rules!)
            .ThenInclude(el => el.Conditions)
            .ThenInclude(el => el.Variable)
        .Include(el => el.Rules!)
            .ThenInclude(el => el.Conclusion)
            .ThenInclude(el => el.Variable)
        .Include(el => el.Variables)
        .FirstOrDefaultAsync();

    return Results.Ok(newProject.ToProjectDTO());
}).RequireAuthorization("Admin");

app.MapDelete("/projects/{id}", async (int id, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
        return Results.NotFound();

    db.Projects.Remove(project);
    await db.SaveChangesAsync();

    return Results.Ok();
}).RequireAuthorization("Admin");
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
}).RequireAuthorization();

app.MapGet("/projects/{projectId}/variables/{id}", async (int projectId, int id, AppDbContext db) =>
{
    var variable = await db.Variables
        .FirstOrDefaultAsync(v => v.ProjectId == projectId && v.Id == id);

    return variable != null ? Results.Ok(variable) : Results.NotFound();
}).RequireAuthorization();

app.MapPost("/projects/{projectId}/variables", async (int projectId, Variable variable, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(projectId);
    if (project == null)
        return Results.NotFound("Projeto não encontrado");

    variable.ProjectId = projectId;

    //barra a criação de uma variável single value com mais de um valor possível
    if (variable.Type == VariableType.SingleValue && variable.PossibleValues.Count > 1)
        return Results.BadRequest("A variável não pode possuir múltiplos valores se ela é univalorada");

    db.Variables.Add(variable);
    await db.SaveChangesAsync();

    return Results.Created($"/projects/{projectId}/variables/{variable.Id}", variable);
}).RequireAuthorization("Admin");

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
}).RequireAuthorization("Admin");

app.MapDelete("/projects/{projectId}/variables/{id}", async (int projectId, int id, AppDbContext db) =>
{
    var variable = await db.Variables
        .FirstOrDefaultAsync(v => v.ProjectId == projectId && v.Id == id);
    if (variable == null)
        return Results.NotFound();

    db.Variables.Remove(variable);
    await db.SaveChangesAsync();

    return Results.Ok();
}).RequireAuthorization("Admin");
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
}).RequireAuthorization();

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
}).RequireAuthorization();

app.MapPost("/projects/{projectId}/rules", async (int projectId, Rule rule, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(projectId);
    if (project == null)
        return Results.NotFound("Projeto não encontrado");

    rule.ProjectId = projectId;

    //verifica se as variáveis de condição existem naquele projeto e se o valor delas está dentro do esperado
    foreach (var condition in rule.Conditions)
    {
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

    var readableRule = Utils.ConvertToReadableRule(rule);

    return Results.Created($"/projects/{projectId}/rules/{rule.Id}", readableRule);
}).RequireAuthorization("Admin");

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
}).RequireAuthorization("Admin");

app.MapDelete("/projects/{projectId}/rules/{id}", async (int projectId, int id, AppDbContext db) =>
{
    var rule = await db.Rules.FirstOrDefaultAsync(r => r.ProjectId == projectId && r.Id == id);
    if (rule == null)
        return Results.NotFound();

    db.Rules.Remove(rule);
    await db.SaveChangesAsync();

    return Results.Ok();
}).RequireAuthorization("Admin");
#endregion

app.UseHttpsRedirection();
app.Run();