using System.Text.Json.Serialization;

namespace EducationalContentGeneration.Core.Models
{
    public class QuestionPaperNoAnswerResponse
    {
        [JsonPropertyName("ExamInfo")]
        public ExamInfo ExamInfo { get; set; } = new();
        [JsonPropertyName("Sections")]
        public QuestionSectionsNoAnswer Sections { get; set; } = new();
    }

    public class QuestionSectionsNoAnswer
    {
        [JsonPropertyName("Mcq")]
        public List<McqQuestionNoAnswer> Mcq { get; set; } = new();
        [JsonPropertyName("ShortAnswers")]
        public List<ShortAnswerQuestionNoAnswer> ShortAnswers { get; set; } = new();
        [JsonPropertyName("LongAnswers")]
        public List<LongAnswerQuestionNoAnswer> LongAnswers { get; set; } = new();
    }

    public class QuestionItem
    {
        [JsonPropertyName("Question")]
        public string Question { get; set; } = string.Empty;
        [JsonPropertyName("Options")]
        public List<string> Options { get; set; } = new();
    }

    public class McqQuestionNoAnswer
    {
        [JsonPropertyName("Question")]
        public string Question { get; set; } = string.Empty;
        [JsonPropertyName("Options")]
        public List<string> Options { get; set; } = new();
    }

    public class ShortAnswerQuestionNoAnswer
    {
        [JsonPropertyName("Question")]
        public string Question { get; set; } = string.Empty;
    }

    public class LongAnswerQuestionNoAnswer
    {
        [JsonPropertyName("Question")]
        public string Question { get; set; } = string.Empty;
    }
}
