namespace DndSolution.Infrastructure.Services
{
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

    public class SpellApiResponse
    {
        public string Index { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string[]? Desc { get; set; }
        public string[]? HigherLevel { get; set; }
        public string Range { get; set; } = string.Empty;
        public string[]? Components { get; set; }
        public string? Material { get; set; }
        public bool? Ritual { get; set; }
        public string Duration { get; set; } = string.Empty;
        public bool? Concentration { get; set; }
        public string CastingTime { get; set; } = string.Empty;
        public int Level { get; set; }
        public ApiReference? School { get; set; }
        public List<ApiReference>? Classes { get; set; }
        public List<ApiReference>? Subclasses { get; set; }
        public string Url { get; set; } = string.Empty;
    }

    public class School : ApiReference { }
    public class ClassReference : ApiReference { }
    public class SubclassReference : ApiReference { }
}