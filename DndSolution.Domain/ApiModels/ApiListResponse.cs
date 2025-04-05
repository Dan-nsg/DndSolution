namespace DndSolution.Domain.ApiModels;
public class ApiListResponse
{
    public int Count { get; set; }
    public List<ApiReference> Results { get; set; } = new();
}

public class ApiReference
{
    public string Index { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}