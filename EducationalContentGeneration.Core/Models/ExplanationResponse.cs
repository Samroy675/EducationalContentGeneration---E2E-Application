using System.Text.Json.Serialization;

namespace EducationalContentGeneration.Core.Models
{
    public class ExplanationResponse
    {
        [JsonPropertyName("Evaluation")]
        public string Evaluation {  get; set; } = string.Empty;
        [JsonPropertyName("Explanation")]
        public string Explanation { get; set; } = string.Empty;
    }
}
