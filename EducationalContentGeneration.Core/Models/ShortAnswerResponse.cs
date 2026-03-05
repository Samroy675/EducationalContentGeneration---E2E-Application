namespace EducationalContentGeneration.Core.Models
{
    public class ShortAnswerResponse
    {
        public List<ShortAnswerQuestion> Questions { get; set; } = new();
    }

    public class ShortAnswerQuestion
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public List<string> KeyPoints { get; set; } = new();
    }
}
