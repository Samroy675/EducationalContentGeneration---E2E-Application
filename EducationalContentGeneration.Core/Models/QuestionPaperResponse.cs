using System.Text.Json.Serialization;

namespace EducationalContentGeneration.Core.Models
{
    public class QuestionPaperResponse
    {
        [JsonPropertyName("ExamInfo")]
        public ExamInfo ExamInfo { get; set; } = new();
        [JsonPropertyName("Sections")]
        public QuestionSections Sections { get; set; } = new();
    }

    public class ExamInfo
    {
        [JsonPropertyName("Subject")]
        public string Subject { get; set; } = string.Empty;
        [JsonPropertyName("ClassLevel")]
        public string ClassLevel {  get; set; } = string.Empty;
        [JsonPropertyName("Difficulty")]
        public string Difficulty {  get; set; } = string.Empty;
        [JsonPropertyName("TotalMarks")]
        public int TotalMarks { get; set; }
        [JsonPropertyName("ExamDuration")]
        public int ExamDuration { get; set; }
    }

    public class QuestionSections
    {
        [JsonPropertyName("Mcq")]
        public List<McqQuestion> Mcq { get; set; } = new();
        [JsonPropertyName("ShortAnswers")]
        public List<ShortAnswerQuestion> ShortAnswers { get; set; } = new();
        [JsonPropertyName("LomgAnswers")]
        public List<LongAnswerQuestion> LongAnswers { get; set; } = new();
    }
}
