using ExsysmaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExsysmaAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<Variable> Variables { get; set; }
    public DbSet<User> Users { get; set; }
}