using Hangfire;
using DndSolution.Application.Interfaces;

namespace DndSolution.Worker.Jobs;

public class DndApiSyncJob
{
    private readonly IDndApiService _dndApiService;

    public DndApiSyncJob(IDndApiService dndApiService)
    {
        _dndApiService = dndApiService;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task SyncData()
    {
        await _dndApiService.SyncSpells();
        //adicione outros métodos de sincronização aqui
    }
}