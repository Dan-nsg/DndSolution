using Microsoft.EntityFrameworkCore;
using DndSolution.Application.Interfaces;
using DndSolution.Domain.Entities;

namespace DndSolution.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Spell> Spells { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}