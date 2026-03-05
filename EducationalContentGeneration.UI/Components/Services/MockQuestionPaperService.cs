using System.Text;
using EducationalContentGeneration.Core.Models;

namespace EducationalContentGeneration.UI.Components.Services
{
    public class MockQuestionPaperService : IQuestionPaperService
    {
        public async Task<string> BuildQuestionPaperAsync(QuestionPaperRequest request)
        {
           await Task.Delay(500);

            var sb = new StringBuilder();

            sb.AppendLine("========================================================================");
            sb.AppendLine("Question Paper");
            sb.AppendLine("========================================================================");

            sb.AppendLine();
            sb.AppendLine($"Subject: {request.Subject}");
            sb.AppendLine($"Topic: {request.Topic}");
            sb.AppendLine($"Class: {request.ClassLevel}");
            sb.AppendLine($"Difficulty: {request.Difficulty}");
            sb.AppendLine($"Time: {request.ExamTime}");
            sb.AppendLine($"Total Marks: {request.TotalMarks}");
            sb.AppendLine();

            if(request.McqCount > 0)
            {
                sb.AppendLine("SECTION A - Multiple Choice Questions");
                sb.AppendLine("--------------------------------------");

                for(int i=1;i<=request.McqCount;i++)
                {
                    sb.AppendLine($"{i}, Sample MCQ Question?");
                    sb.AppendLine("A) Option A");
                    sb.AppendLine("A) Option B");
                    sb.AppendLine("A) Option C");
                    sb.AppendLine("A) Option D");

                    if(request.IncludeAnswers) sb.AppendLine("Answer: A");

                    sb.AppendLine();
                }
            }

            if(request.ShortAnswerCount > 0)
            {
                sb.AppendLine("SECTION B - Short Answer Questions");
                sb.AppendLine("-------------------------------------");

                for(int i=1;i<=request.ShortAnswerCount;i++)
                {
                    sb.AppendLine($"{i}, Sample Short Questions?");

                    if (request.IncludeAnswers) sb.AppendLine("Answer: Sample short answer in 3-5 lines.");

                    sb.AppendLine();
                }
            }

            if (request.LongAnswerCount > 0)
            {
                sb.AppendLine("SECTION C - Long Answer Questions");
                sb.AppendLine("-------------------------------------");

                for (int i = 1; i <= request.LongAnswerCount; i++)
                {
                    sb.AppendLine($"{i}, Sample Long Questions?");

                    if (request.IncludeAnswers) sb.AppendLine("Answer: Sample long answer explaining the concept.");

                    sb.AppendLine();
                }
            }

            sb.AppendLine("========================================================================");
            sb.AppendLine("END OF QUESTION PAPER");

           return sb.ToString();
        }
    }
}
