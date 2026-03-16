using System.Text.Json.Serialization;

namespace EducationalContentGeneration.Core.Models
{
    public class McqResponse
    {
        [JsonPropertyName("Questions")]
        public List<McqQuestion> questions { get; set; } = new();
    }

    public class McqQuestion
    {
        [JsonPropertyName("Question")]
        public string question { get; set; } = string.Empty;
        [JsonPropertyName("Options")]
        public List<string> options { get; set; } = new();
        [JsonPropertyName("CorrectAnswer")]
        public string correctAnswer { get; set; } = string.Empty;
        [JsonPropertyName("Explanation")]
        public string explanation { get; set; } = string.Empty;
    }
}
