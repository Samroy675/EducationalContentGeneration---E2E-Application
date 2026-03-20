using EducationalContentGeneration.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EducationalContentGeneration.Core.Models
{
    public class ContentGenerationRequest
    {
        [Required]
        public ContentGenerationType ContentType { get; set; }

        [Required]
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = default!;

        [Required]
        [JsonPropertyName("classLevel")]
        public EducationClass ClassLevel { get; set; }

        [Required]
        [JsonPropertyName("difficultyLevel")]
        public DifficultyLevel DifficultyLevel { get; set; }

        [JsonPropertyName("topic")]
        public string? Topic { get; set; }

        [Range(0, 50)]
        [JsonPropertyName("numberOfQuestions")]
        public int? NumberOfQuestions { get; set; }

        [Range(0, 50)]
        [JsonPropertyName("mcqCount")]
        public int? McqCount { get; set; }

        [Range(0, 50)]
        [JsonPropertyName("shortAnswerCount")]
        public int? ShortAnswerCount { get; set; }

        [Range(0, 50)]
        [JsonPropertyName("longAnswerCount")]
        public int? LongAnswerCount { get; set; }

        [Range(0, 100)]
        [JsonPropertyName("totalMarks")]
        public int? TotalMarks { get; set; }

        [Range(0, 180)]
        [JsonPropertyName("examDuration")]
        public int? ExamDuration { get; set; }

        [JsonPropertyName("includeAnswers")]
        public bool IncludeAnswers { get; set; } = true;

        [JsonPropertyName("question")]
        public string? Question { get; set; }

        [JsonPropertyName("answer")]
        public string? Answer { get; set; }

    }
}
