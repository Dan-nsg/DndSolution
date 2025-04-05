using System.ComponentModel.DataAnnotations;

namespace DndSolution.Domain.Entities
{
    public class Spell
    {
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Index { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string HigherLevel { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Range { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Components { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Material { get; set; } = string.Empty;

        public bool Ritual { get; set; }

        [Required, MaxLength(50)]
        public string Duration { get; set; } = string.Empty;

        public bool Concentration { get; set; }

        [Required, MaxLength(50)]
        public string CastingTime { get; set; } = string.Empty;

        public int Level { get; set; }

        [Required, MaxLength(50)]
        public string School { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Classes { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Subclasses { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Url { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}