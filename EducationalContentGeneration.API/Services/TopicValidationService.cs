using EducationalContentGeneration.Core.Prompting;
using Microsoft.SemanticKernel;

namespace EducationalContentGeneration.API.Services
{
    public class TopicValidationService
    {
        private readonly Kernel _kernel;
        private readonly IPromptLoader _promptLoader;

        public TopicValidationService(Kernel kernel, IPromptLoader promptLoader)
        {
            _kernel = kernel;
            _promptLoader = promptLoader;
        }

        public async Task<int> GetTopicScoreAsync(string subject, string topic)
        {
            var promptTemplate = await _promptLoader.LoadAsync("topic-validation");

            var function = _kernel.CreateFunctionFromPrompt(promptTemplate);

            var result = await _kernel.InvokeAsync(function, new KernelArguments
            {
                ["subject"] = subject,
                [topic] = topic
            });

            if(int.TryParse(result.ToString(), out var score)) return score;

            return 0;
        }
    }
}
