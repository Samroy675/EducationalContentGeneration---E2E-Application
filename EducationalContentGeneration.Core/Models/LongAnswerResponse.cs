namespace EducationalContentGeneration.Core.Models
{
    public class LongAnswerResponse
    {
        public List<LongAnswerQuestion> Questions { get; set; } = new();
    }

    public class LongAnswerQuestion
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
