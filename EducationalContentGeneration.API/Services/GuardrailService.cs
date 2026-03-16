using EducationalContentGeneration.Core.Prompting;
using Microsoft.SemanticKernel;

namespace EducationalContentGeneration.API.Services
{
    public class GuardrailService
    {
        private readonly Kernel _kernel;
        private readonly IPromptLoader _promptLoader;

        public GuardrailService(Kernel kernel, IPromptLoader promptLoader)
        {
            _promptLoader = promptLoader;
            _kernel = kernel;
        }

        public async Task<int> GetPromptScoreAsync(string prompt)
        {
            var promptTemplate = await _promptLoader.LoadAsync("guardrail");

            var function = _kernel.CreateFunctionFromPrompt(promptTemplate);

            var result = await _kernel.InvokeAsync(function, new KernelArguments
            {
                ["prompt"] = prompt
            });

            if (int.TryParse(result.ToString(), out int score)) return score;

            return 0;
        }
    }
}
