using EducationalContentGeneration.Core.Prompting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using System.ComponentModel;

namespace EducationalContentGeneration.Core.Plugins
{
    public class ContentGenerationPlugin
    {
        private readonly IPromptLoader _promptLoader;
        private readonly Kernel _kernel;
        private readonly ILogger<ContentGenerationPlugin> _logger;

        // We inject Kernel so we can invoke a semantic function
        public ContentGenerationPlugin(IPromptLoader promptLoader, Kernel kernel, ILogger<ContentGenerationPlugin> logger)
        {
            _promptLoader = promptLoader ?? throw new ArgumentNullException(nameof(promptLoader));
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _logger = logger;
        }

        [KernelFunction("GenerateMcq")]
        [Description("Generates multiple-choice questions (MCQs).")]
        public async Task<string> GenerateMcqAsync(
           string subject,
           string topic,
           string difficultyLevel,
           string classLevel,
           int numberOfQuestions)
        {
            ValidateCommon(subject, topic, numberOfQuestions);

            var template = await _promptLoader.LoadAsync("mcq");

            var args = new KernelArguments
            {
                ["subject"] = subject,
                ["topic"] = topic,
                ["difficultyLevel"] = difficultyLevel,
                ["classLevel"] = classLevel,
                ["numberOfQuestions"] = numberOfQuestions
            };

            var response = await ExecutePromptAsync(template, args);

            _logger.LogInformation("Generated JSON response {Response}", response);
            _logger.LogInformation("Generated response for {Function}", nameof(GenerateMcqAsync));

            return response!;
        }

        [KernelFunction("GenerateShortAnswer")]
        [Description("Generates short answer questions.")]
        public async Task<string> GenerateShortAnswerAsync(
            string subject,
            string topic,
            string difficultyLevel,
            string classLevel,
            int numberOfQuestions)
        {
            ValidateCommon(subject, topic, numberOfQuestions);

            var template = await _promptLoader.LoadAsync("short-Answer");

            var args = new KernelArguments
            {
                ["subject"] = subject,
                ["topic"] = topic,
                ["difficultyLevel"] = difficultyLevel,
                ["classLevel"] = classLevel,
                ["numberOfQuestions"] = numberOfQuestions
            };

            var response = await ExecutePromptAsync(template, args);

            _logger.LogInformation("Generated JSON response {Response}", response);
            _logger.LogInformation("Generated response for {Function}", nameof(GenerateShortAnswerAsync));

            return response!;
        }

        [KernelFunction("GenerateLongAnswer")]
        [Description("Generates long answer questions.")]
        public async Task<string> GenerateLongAnswerAsync(
           string subject,
           string topic,
           string difficultyLevel,
           string classLevel,
           int numberOfQuestions)
        {
            ValidateCommon(subject, topic, numberOfQuestions);

            var template = await _promptLoader.LoadAsync("long-Answer");

            var args = new KernelArguments
            {
                ["subject"] = subject,
                ["topic"] = topic,
                ["difficultyLevel"] = difficultyLevel,
                ["classLevel"] = classLevel,
                ["numberOfQuestions"] = numberOfQuestions
            };

            var response = await ExecutePromptAsync(template, args);

            _logger.LogInformation("Generated JSON response {Response}", response);
            _logger.LogInformation("Generated response for {Function}", nameof(GenerateLongAnswerAsync));

            return response!;
        }

        [KernelFunction("EvaluateExplanation")]
        [Description("Evaluates an answer and provides explanation.")]
        public async Task<string> EvaluateExplanationAsync(
           string question, string answer)
        {
            ValidateExplanation(question, answer);

            var template = await _promptLoader.LoadAsync("explanation");

            var args = new KernelArguments
            {
                ["question"] = question,
                ["answer"] = answer
            };

            var response = await ExecutePromptAsync(template, args);

            _logger.LogInformation("Generated JSON response {Response}", response);
            _logger.LogInformation("Generated response for {Function}", nameof(EvaluateExplanationAsync));

            return response!;
        }

        [KernelFunction("BuildQuestionPaper")]
        [Description("Generates a full question paper.")]
        public async Task<string> BuildQuestionPaperAsync(
           string subject,
           string difficultyLevel,
           string classLevel,
           int mcqCount,
           int shortAnswerCount,
           int longAnswerCount,
           string examDuration,
           string totalMarks,
           bool includeAnswers)
        {
            ValidateQuestionPaper(subject, mcqCount, shortAnswerCount, longAnswerCount);

            var template = await _promptLoader.LoadAsync("question-paper");

            var args = new KernelArguments
            {
                ["subject"] = subject,
                ["difficultyLevel"] = difficultyLevel,
                ["classLevel"] = classLevel,
                ["mcqCount"] = mcqCount,
                ["shortAnswerCount"] = shortAnswerCount,
                ["longAnswerCount"] = longAnswerCount,
                ["examDuration"] = examDuration,
                ["totalMarks"] = totalMarks,
                ["includeAnswers"] = includeAnswers
            };

            var response = await ExecutePromptAsync(template, args);

            _logger.LogInformation("Generated JSON response {Response}", response);
            _logger.LogInformation("Generated response for {Function}", nameof(BuildQuestionPaperAsync));

            return response!;
        }

        private async Task<string> ExecutePromptAsync(string template, KernelArguments args)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new InvalidOperationException("Prompt template is empty.");

            var settings = new AzureOpenAIPromptExecutionSettings
            {
                Temperature = 0.1,
                TopP = 0.8,
                ResponseFormat = "json_object"
            };

            args.ExecutionSettings = new Dictionary<string, PromptExecutionSettings>
            {
                ["default"] = settings
            };
            var result = await _kernel.InvokePromptAsync(template, args);

            _logger.LogInformation("Generated response for {Function}", nameof(ExecutePromptAsync));

            return result.GetValue<string>() ?? string.Empty;
        }

        private static void ValidateCommon(string subject, string topic, int numberOfQuestions)
        {
            if(string.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Subject cannot be empty.", nameof(subject));
            if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentException("Topic cannot be empty.", nameof(topic));
            if (numberOfQuestions <= 0 || numberOfQuestions > 50) throw new ArgumentOutOfRangeException(nameof(numberOfQuestions), "Must be between 1 and 50");
        }

        private static void ValidateExplanation(string question, string answer)
        {
            if(string.IsNullOrWhiteSpace(question)) throw new ArgumentException("Question cannot be empty.", nameof(question));
            if(string.IsNullOrWhiteSpace(answer)) throw new ArgumentException("Answer cannot be empty.", nameof(answer));
        }

        private static void ValidateQuestionPaper(string subject, int mcq, int shortAns, int longAns)
        {
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Subject cannot be empty.", nameof(subject));
            if (mcq < 0 || shortAns < 0 || longAns < 0) throw new ArgumentOutOfRangeException("Question counts cannot be negative.");
        }
    }
}
