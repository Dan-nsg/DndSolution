using Microsoft.EntityFrameworkCore;
using DndSolution.Infrastructure.Persistence;
using DndSolution.Application.Interfaces;
using DndSolution.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            sqlOptions.CommandTimeout(60);
        }));

builder.Services.AddHttpClient<IDndApiService, DndApiService>();
builder.Services.AddScoped<IDndApiService, DndApiService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.MapPost("/api/spells/sync", async (IDndApiService service) =>
{
    await service.SyncSpells();
    return Results.Ok(new { message = "Sincronização iniciada" });
});

app.MapGet("/api/spells", async (
    IAppDbContext context,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    [FromQuery] string? search = null,
    [FromQuery] int? level = null,
    [FromQuery] string? school = null) =>
{
    var query = context.Spells.AsQueryable();

    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(s =>
            s.Name.Contains(search) ||
            s.Description.Contains(search));
    }

    if (level.HasValue)
    {
        query = query.Where(s => s.Level == level.Value);
    }

    if (!string.IsNullOrEmpty(school))
    {
        query = query.Where(s => s.School == school);
    }

    var total = await query.CountAsync();
    var spells = await query
        .OrderBy(s => s.Name)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Results.Ok(new
    {
        total,
        page,
        pageSize,
        data = spells
    });
});

app.MapGet("/api/spells/{id}", async (
    IAppDbContext context,
    Guid id) =>
{
    var spell = await context.Spells.FindAsync(id);
    return spell != null ? Results.Ok(spell) : Results.NotFound();
});

app.Run();