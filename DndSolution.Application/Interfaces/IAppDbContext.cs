using DndSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DndSolution.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Spell> Spells { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}