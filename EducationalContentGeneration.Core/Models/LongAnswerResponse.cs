using System.Text.Json.Serialization;

namespace EducationalContentGeneration.Core.Models
{
    public class LongAnswerResponse
    {
        [JsonPropertyName("Questions")]
        public List<LongAnswerQuestion> Questions { get; set; } = new();
    }

    public class LongAnswerQuestion
    {
        [JsonPropertyName("Question")]
        public string Question { get; set; } = string.Empty;
        [JsonPropertyName("Answer")]
        public string Answer { get; set; } = string.Empty;
    }
}
