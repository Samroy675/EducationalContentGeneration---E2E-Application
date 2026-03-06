using EducationalContentGeneration.Core.Models;
using EducationalContentGeneration.Core.Prompting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using System.ComponentModel;
using System.Text.Json;

namespace EducationalContentGeneration.Core.Plugins
{
    public class ContentGenerationPlugin
    {
        private readonly IPromptLoader _promptLoader;
        private readonly Kernel _kernel;

        // We inject Kernel so we can invoke a semantic function
        public ContentGenerationPlugin(IPromptLoader promptLoader, Kernel kernel)
        {
            _promptLoader = promptLoader ?? throw new ArgumentNullException(nameof(promptLoader));
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        [KernelFunction("GenerateMcq")]
        [Description("Use this function when the user requests multiple choicw Questions (MCQs). It generates Questions with four options and the correct Answer.")]
        public async Task<string> GenerateMcqAsync(
           string subject,
           string topic,
           string difficultyLevel,
           string classLevel,
           int numberOfQuestions)
        {
            //Basic backend-level validation
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Subject cannot be empty.", nameof(subject));

            if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentException("Topic cannot be empty.", nameof(topic));

            if (numberOfQuestions <= 0 || numberOfQuestions > 50) throw new ArgumentOutOfRangeException(nameof(numberOfQuestions), "Number of Questions must be between 1 and 50.");

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

            Console.WriteLine("\nJsonResponse");
            Console.WriteLine(response);

            return response!;
        }

        [KernelFunction("GenerateShortAnswer")]
        [Description("Use this function when the user requests short Answer Questions that require brief responses or explanations.")]
        public async Task<string> GenerateShortAnswerAsync(
            string subject,
            string topic,
            string difficultyLevel,
            string classLevel,
            int numberOfQuestions)
        {
            //Basic backend-level validation
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Subject cannot be empty.", nameof(subject));

            if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentException("Topic cannot be empty.", nameof(topic));

            if (numberOfQuestions <= 0 || numberOfQuestions > 50) throw new ArgumentOutOfRangeException(nameof(numberOfQuestions), "Number of Questions must be between 1 and 50.");

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

            Console.WriteLine("\nJsonResponse");
            Console.WriteLine(response);

            return response!;
        }

        [KernelFunction("GenerateLongAnswer")]
        [Description("Use this function when the user requests long descriptions Questions that require detailed explanations or paragraph-length Answers.")]
        public async Task<string> GenerateLongAnswerAsync(
           string subject,
           string topic,
           string difficultyLevel,
           string classLevel,
           int numberOfQuestions)
        {
            //Basic backend-level validation
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Subject cannot be empty.", nameof(subject));

            if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentException("Topic cannot be empty.", nameof(topic));

            if (numberOfQuestions <= 0 || numberOfQuestions > 50) throw new ArgumentOutOfRangeException(nameof(numberOfQuestions), "Number of Questions must be between 1 and 50.");

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

            Console.WriteLine("\nJsonResponse");
            Console.WriteLine(response);

            return response!;
        }

        [KernelFunction("EvaluateExplanation")]
        [Description("Use this function when a student Answer needs to be evaluated. It checks whether the provided Answer is correct and explains the reasoning.")]
        public async Task<string> EvaluateExplanationAsync(
           string subject,
           string question, string answer)
        {
            //minimal backend-level validation
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Subject cannot be empty.", nameof(subject));

            if (string.IsNullOrWhiteSpace(question)) throw new ArgumentException("Question cannot be empty.", nameof(question));

            if (string.IsNullOrWhiteSpace(answer)) throw new ArgumentException("Answer cannot be empty.", nameof(answer));

            if (string.IsNullOrEmpty(question) || string.IsNullOrEmpty(answer))
            {
                throw new ArgumentException("Question and Answer are required for explanation generation");
            }

            var template = await _promptLoader.LoadAsync("explanation");

            var args = new KernelArguments
            {
                ["subject"] = subject,
                ["question"] = question,
                ["answer"] = answer
            };

            var response = await ExecutePromptAsync(template, args);

            Console.WriteLine("\nJsonResponse");
            Console.WriteLine(response);

            return response!;
        }

        [KernelFunction("BuildQuestionPaper")]
        [Description("Use this function when the user wants a full exam Question paper containing MCQs, short Answer Questions, and long Answer Questions.")]
        public async Task<string> BuildQuestionPaperAsync(
           string subject,
           string topic,
           string difficultyLevel,
           string classLevel,
           int mcqCount,
           int shortAnswerCount,
           int longAnswerCount,
           string examDuration,
           string totalMarks,
           bool includeAnswers)
        {
            //Basic backend-level validation
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Subject cannot be empty.", nameof(subject));

            if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentException("Topic cannot be empty.", nameof(topic));

            if (mcqCount < 0 || shortAnswerCount < 0 || longAnswerCount < 0) throw new ArgumentOutOfRangeException("Question counts cannot be negative.");

            var template = await _promptLoader.LoadAsync("question-paper");

            var args = new KernelArguments
            {
                ["subject"] = subject,
                ["topic"] = topic,
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

            Console.WriteLine("\nJsonResponse");
            Console.WriteLine(response);

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

            return result.GetValue<string>() ?? string.Empty;
        }
    }
}
