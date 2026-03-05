using EducationalContentGeneration.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EducationalContentGeneration.Core.Models
{
    public class ContentGenerationRequest
    {
        [Required]
        public ContentGenerationType ContentType { get; set; }

        [Required]
        public string subject { get; set; } = default!;

        [Required]
        public EducationClass classLevel { get; set; }

        [Required]
        public DifficultyLevel difficultyLevel { get; set; }

        public string? topic { get; set; }

        [Range(0, 50)]
        public int? numberOfQuestions { get; set; }

        [Range(0, 50)]
        public int? mcqCount { get; set; }

        [Range(0, 50)]
        public int? shortAnswerCount { get; set; }

        [Range(0, 50)]
        public int? longAnswerCount { get; set; }

        [Range(0, 100)]
        public int? totalMarks { get; set; }

        [Range(0, 180)]
        public int? examDuration { get; set; }

        public bool? includeAnswers { get; set; }

        public string? question { get; set; }

        public string? answer { get; set; }

    }
}
