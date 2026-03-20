using EducationalContentGeneration.Core.Enums;
using EducationalContentGeneration.Core.Models;
using EducationalContentGeneration.Core.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text.Json;

namespace EducationalContentGeneration.API.Services
{
    public class KernelService
    {
        private readonly Kernel _kernel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KernelService> _logger;
        public KernelService(Kernel kernel, IServiceProvider serviceProvider, ILogger<KernelService> logger)
        {
            _kernel = kernel;
            _serviceProvider = serviceProvider;
            _logger = logger;

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

        public async Task<PromptResponse> GeneratePromptContentAsync(string prompt)
        {
            EnsurePluginsAdded();

            var settings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments(settings));

            _logger.LogInformation("Prompt executed {Prompt}", prompt);

            var responseText = result.GetValue<string>();

            return new PromptResponse
            {
                Message = responseText ?? string.Empty
            };
        }

        public async Task<object> GenerateContentAsync(ContentGenerationRequest request)
        {
            Type responseType = request.ContentType switch
            {
                ContentGenerationType.Mcq => typeof(McqResponse),
                ContentGenerationType.ShortAnswer => typeof(ShortAnswerResponse),
                ContentGenerationType.LongAnswer => typeof(LongAnswerResponse),
                ContentGenerationType.Explanation => typeof(ExplanationResponse),
                ContentGenerationType.QuestionPaper => request.IncludeAnswers == true
                ? typeof(QuestionPaperResponse)
                : typeof(QuestionPaperNoAnswerResponse),

                _ => throw new InvalidOperationException("Unsupported content type.")
            };

            EnsurePluginsAdded();

            var prompt = BuildPrompt(request).Trim();

            var settings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                ResponseFormat = responseType
            };

            var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments(settings));

            var responseText = result.GetValue<string>() ?? string.Empty;

            var responseObject = JsonSerializer.Deserialize(responseText, responseType)!;

            return responseObject;
        }

        private string BuildPrompt(ContentGenerationRequest request)
        {
            var subject = request.Subject ??= "General Subject";
            var topic = request.Topic ??= "General Topic";
            var classLevel = request.ClassLevel == default ? EducationClass.Class6 : request.ClassLevel;
            var difficulty = request.ClassLevel == default ? DifficultyLevel.Medium : request.DifficultyLevel;
            var count = request.NumberOfQuestions = request.NumberOfQuestions <= 0 ? 1 : request.NumberOfQuestions;
            var includeAnswers = request.IncludeAnswers ? true : false;

            var answerInstruction = includeAnswers 
                ? "Include correct answers and explanations." 
                : "Do NOT include answers.";

            return request.ContentType switch
            {
                ContentGenerationType.Mcq =>
                $"Generate {count} MCQs for Subject {subject} on Topic {topic} for class {classLevel} with difficulty {difficulty}.",

                ContentGenerationType.ShortAnswer =>
                $"Generate {count} short Answer questions for Subject {subject} on Topic {topic} for class {classLevel} with difficulty {difficulty}.",

                ContentGenerationType.LongAnswer =>
                $"Generate {count} long Answer questions for Subject {subject} on Topic {topic} for class {classLevel} with difficulty {difficulty}.",

                ContentGenerationType.Explanation =>
                $"Evaluate this answer. Question: {request.Question}. Answer: {request.Answer}.",

                ContentGenerationType.QuestionPaper =>
                $@"Generate a question paper for Subject {subject} 
                (Class {classLevel}, difficulty{difficulty}). 
                 Include:
                 - {request.McqCount} MCQs
                 - {request.ShortAnswerCount} short answers
                 - {request.LongAnswerCount} long answers
                 Total marks: {request.TotalMarks}, Duration: {request.ExamDuration} minutes.
                 {answerInstruction}",

                _ => throw new InvalidOperationException("Unsupported content type.")
            };
        }
    }
}

