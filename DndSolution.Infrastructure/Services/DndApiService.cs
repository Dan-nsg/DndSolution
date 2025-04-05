using System.Net.Http.Json;
using DndSolution.Domain.Entities;
using DndSolution.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DndSolution.Infrastructure.Services
{
    public class DndApiService : IDndApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IAppDbContext _context;
        private readonly ILogger<DndApiService> _logger;

        public DndApiService(
            HttpClient httpClient,
            IAppDbContext context,
            ILogger<DndApiService> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://www.dnd5eapi.co");
        }

        public async Task SyncSpells()
        {
            try
            {
                _logger.LogInformation("Starting spells synchronization...");

                var response = await _httpClient.GetFromJsonAsync<ApiListResponse>("/api/spells");

                if (response?.Results != null)
                {
                    foreach (var spellRef in response.Results)
                    {
                        var existing = await _context.Spells
                            .FirstOrDefaultAsync(s => s.Index == spellRef.Index);

                        if (existing == null)
                        {
                            var spellResponse = await _httpClient
                                .GetFromJsonAsync<SpellApiResponse>($"/api/spells/{spellRef.Index}");

                            if (spellResponse != null)
                            {
                                var spell = new Spell
                                {
                                    Id = Guid.NewGuid(),
                                    Index = spellResponse.Index,
                                    Name = spellResponse.Name,
                                    Description = ProcessDescription(spellResponse.Desc),
                                    HigherLevel = ProcessDescription(spellResponse.HigherLevel),
                                    Range = spellResponse.Range,
                                    Components = spellResponse.Components != null ?
                                        string.Join(",", spellResponse.Components) : string.Empty,
                                    Material = spellResponse.Material ?? string.Empty,
                                    Ritual = spellResponse.Ritual ?? false,
                                    Duration = spellResponse.Duration,
                                    Concentration = spellResponse.Concentration ?? false,
                                    CastingTime = spellResponse.CastingTime,
                                    Level = spellResponse.Level,
                                    School = spellResponse.School?.Name ?? string.Empty,
                                    Classes = spellResponse.Classes != null ?
                                        string.Join(",", spellResponse.Classes.Select(c => c.Name)) : string.Empty,
                                    Subclasses = spellResponse.Subclasses != null ?
                                        string.Join(",", spellResponse.Subclasses.Select(s => s.Name)) : string.Empty,
                                    Url = spellResponse.Url,
                                    CreatedAt = DateTime.UtcNow
                                };

                                await _context.Spells.AddAsync(spell);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation("Spells synchronization completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during spells synchronization");
                throw;
            }
        }

        private string ProcessDescription(string[]? descriptionLines)
        {
            if (descriptionLines == null || descriptionLines.Length == 0)
                return string.Empty;

            return string.Join("\n\n", descriptionLines
                .Where(line => !string.IsNullOrWhiteSpace(line)));
        }
    }
}