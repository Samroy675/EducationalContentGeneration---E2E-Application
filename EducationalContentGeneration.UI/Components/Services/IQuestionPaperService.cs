using EducationalContentGeneration.Core.Models;

namespace EducationalContentGeneration.UI.Components.Services
{
    public interface IQuestionPaperService
    {
        Task<string> BuildQuestionPaperAsync(QuestionPaperRequest request);
    }
}
