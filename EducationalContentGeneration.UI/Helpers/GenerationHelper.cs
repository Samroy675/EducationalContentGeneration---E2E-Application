using EducationalContentGeneration.Core.Models;

namespace EducationalContentGeneration.UI.Helpers
{
    public static class GenerationHelper
    {
        public static string BuildMcqText(List<McqQuestion> questions)
        {
            return string.Join("\n\n", questions.Select((q, i) =>
                $"Q{i + 1}. {q.Question}\n" +
                string.Join("\n", q.Options.Select((o, j) => $"  {(char)('A' + j)}) {o}")) +
                $"\nAnswer: {q.CorrectAnswer}"
            ));
        }

        public static string BuildShortAnswerText(List<ShortAnswerQuestion> questions)
        {
            return string.Join("\n\n", questions.Select((q, i) =>
                $"Q{i + 1}. {q.Question}\nAnswer: {q.Answer}"
            ));
        }

        public static string BuildLongAnswerText(List<LongAnswerQuestion> questions)
        {
            return string.Join("\n\n", questions.Select((q, i) =>
                $"Q{i + 1}. {q.Question}\n\nAnswer: {q.Answer}"
            ));
        }
    }
}
