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
            if(string.IsNullOrWhiteSpace(prompt)) throw new ArgumentException("Prompt cannot be null or empty", nameof(prompt));

            var promptTemplate = await _promptLoader.LoadAsync("guardrail");

            var function = _kernel.CreateFunctionFromPrompt(promptTemplate);

            var result = await _kernel.InvokeAsync(function, new KernelArguments
            {
                ["prompt"] = prompt
            });

            var resultText = result.ToString();

            if (!int.TryParse(resultText, out int promptScore))
            {
                Console.WriteLine($"[Guardrail] Failed to parse score. LLM response: {resultText}");
                return 0;
            }

            return promptScore;
        }
    }
}
