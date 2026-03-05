using EducationalContentGeneration.Core.Models;
using EducationalContentGeneration.Core.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace EducationalContentGeneration.API.Services
{
    public class KernelService
    {
        private readonly Kernel _kernel;
        private readonly IServiceProvider _serviceProvider;
        public KernelService(Kernel kernel, IServiceProvider serviceProvider)
        {
            _kernel = kernel;
            _serviceProvider = serviceProvider;

            EnsurePluginsAdded();
        }

        private bool _pluginsAdded = false;

        private void EnsurePluginsAdded()
        {
            if (_pluginsAdded) return;
            var plugin = _serviceProvider.GetRequiredService<ContentGenerationPlugin>();
            _kernel.Plugins.AddFromObject(plugin);
            _pluginsAdded = true;
        }

        public async Task<string> GeneratePromptContentAsync(string prompt)
        {
            EnsurePluginsAdded();

            var settings = new OpenAIPromptExecutionSettings
            {
                Temperature = 0.1,
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments(settings));

            return result.GetValue<string>() ?? string.Empty;
        }

        public async Task<string> GenerateContentAsync(ContentGenerationRequest request)
        {
            EnsurePluginsAdded();

            var prompt = BuildPrompt(request).Trim();

            var settings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments(settings));
            var responseText = result.GetValue<string>() ?? string.Empty;

            return responseText.Trim();
        }

        private string BuildPrompt(ContentGenerationRequest request)
        {
            request.subject ??= "General Subject";
            request.topic ??= "General Topic";
            if (request.classLevel == default)
            {
                request.classLevel = Core.Enums.EducationClass.Class6;
            }
            if (request.difficultyLevel == default)
            {
                request.difficultyLevel = Core.Enums.DifficultyLevel.Medium;
            }
            request.numberOfQuestions = request.numberOfQuestions <= 0 ? 1 : request.numberOfQuestions;

            return request.ContentType switch
            {
                Core.Enums.ContentGenerationType.Mcq =>
                $"Content type: MCQ. Generate {request.numberOfQuestions} multiple choice Questions for Subject {request.subject} on Topic {request.topic} for class {request.classLevel} with difficulty {request.difficultyLevel}.",

                Core.Enums.ContentGenerationType.ShortAnswer =>
                $"Content type: ShortAnswer. Generate {request.numberOfQuestions} short Answer Questions for Subject {request.subject}  on Topic   {request.topic}   for class   {request.classLevel}   with difficulty  {request.difficultyLevel}.",

                Core.Enums.ContentGenerationType.LongAnswer =>
                $"Content type: LongAnswer. Generate {request.numberOfQuestions} long Answer Questions for Subject {request.subject}  on Topic  {request.topic}  for class  {request.classLevel}  with difficulty  {request.difficultyLevel}.",

                Core.Enums.ContentGenerationType.Explanation =>
                $"Evaluate the following Answer. Subject {request.subject}, Question: {request.question}, Answer: {request.answer}.",

                Core.Enums.ContentGenerationType.QuestionPaper =>
                $"Generate a Question paper for Subject {request.subject} for class {request.classLevel} with difficulty {request.difficultyLevel}. Include {request.mcqCount} MCQs, {request.shortAnswerCount}, short Answer Questions, and {request.longAnswerCount} long Answer Questions. Total marks {request.totalMarks}, duration {request.examDuration}, minutes. Include Answers: {request.includeAnswers}",

                _ => throw new Exception("Unsupported content type")
            };
        }
    }
}
