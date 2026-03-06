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

        public async Task<PromptResponse> GeneratePromptContentAsync(string prompt)
        {
            EnsurePluginsAdded();

            var settings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments(settings));

            Console.WriteLine(result);
            Console.WriteLine(prompt);

            var responseText = result.GetValue<string>();

            return new PromptResponse
            {
                Message = responseText ?? string.Empty
            };
        }

        public async Task<Object> GenerateContentAsync(ContentGenerationRequest request)
        {
            Type responseType = request.ContentType switch
            {
                ContentGenerationType.Mcq => typeof(McqResponse),
                ContentGenerationType.ShortAnswer => typeof(ShortAnswerResponse),
                ContentGenerationType.LongAnswer => typeof(LongAnswerResponse),
                ContentGenerationType.Explanation => typeof(ExplanationResponse),
                ContentGenerationType.QuestionPaper => request.includeAnswers == true
                ? typeof(QuestionPaperResponse)
                : typeof(QuestionPaperNoAnswerResponse),

                _ => throw new Exception("Unsupported Content Type")
            };

            EnsurePluginsAdded();

            var prompt = BuildPrompt(request).Trim();

            var settings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                ResponseFormat = responseType
            };

            var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments(settings));

            var responseText = result.GetValue<string>();

            var responseObject = JsonSerializer.Deserialize(responseText, responseType)!;

            return responseObject;
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

            var answerInstruction = (bool)request.includeAnswers! ? "Include correct answers and explanations for each question." : "Do NOT include answers or explanations. Generate only the questions";

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
                $@"Generate a question paper for Subject {request.subject} 
                for class {request.classLevel} with difficulty{request.difficultyLevel}. 

                 Include {request.mcqCount} MCQs,
                 {request.shortAnswerCount} Short Answer Questions,
                 and {request.longAnswerCount} Long Answer Questions.

                 Total marks {request.totalMarks}, duration {request.examDuration} minutes.
                 {answerInstruction}",

                _ => throw new Exception("Unsupported content type")
            };
        }
    }
}

