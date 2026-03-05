namespace EducationalContentGeneration.Core.Models
{
   public class McqResponse
    {
        public List<McqQuestion> questions { get; set; } = new();
    }

    public class McqQuestion
    {
        public string question { get; set; } = string.Empty;
        public List<string> options { get; set; } = new();
        public string correctAnswer { get; set; } = string.Empty;
        public string explanation { get; set; } = string.Empty;
    }
}
