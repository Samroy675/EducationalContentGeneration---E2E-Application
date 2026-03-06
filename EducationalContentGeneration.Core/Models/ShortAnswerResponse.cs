using System.Text.Json.Serialization;

namespace EducationalContentGeneration.Core.Models
{
    public class ShortAnswerResponse
    {
        [JsonPropertyName("Questions")]
        public List<ShortAnswerQuestion> Questions { get; set; } = new();
    }

    public class ShortAnswerQuestion
    {
        [JsonPropertyName("Question")]
        public string Question { get; set; } = string.Empty;
        [JsonPropertyName("Answer")]
        public string Answer { get; set; } = string.Empty;
        [JsonPropertyName("KeyPoints")]
        public List<string> KeyPoints { get; set; } = new();
    }
}
