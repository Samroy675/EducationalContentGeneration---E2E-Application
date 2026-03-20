using System.Text.Json.Serialization;

namespace EducationalContentGeneration.Core.Models
{
    public class McqResponse
    {
        [JsonPropertyName("Questions")]
        public List<McqQuestion> Questions { get; set; } = new();
    }

    public class McqQuestion
    {
        [JsonPropertyName("Question")]
        public string Question { get; set; } = string.Empty;
        [JsonPropertyName("Options")]
        public List<string> Options { get; set; } = new();
        [JsonPropertyName("CorrectAnswer")]
        public string CorrectAnswer { get; set; } = string.Empty;
        [JsonPropertyName("Explanation")]
        public string Explanation { get; set; } = string.Empty;
    }
}
