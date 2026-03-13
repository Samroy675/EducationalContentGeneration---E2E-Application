using EducationalContentGeneration.API.Services;
using EducationalContentGeneration.Core.Models;

namespace EducationalContentGeneration.API.Endpoints
{
    public static class ContentEndpoints
    {
        public static void MapContentEndpoints(this WebApplication app)
        {
            app.MapPost("/api/content/generate", async Task<IResult> (ContentGenerationRequest request, KernelService kernelService,
                TopicValidationService topicValidationService) => 
               {  
                var topicScore = await topicValidationService.GetTopicScoreAsync(request.subject, request.topic);
                if(topicScore < 7)
                {
                    return TypedResults.BadRequest(new 
                    {
                        message = $"The topic '{request.topic}' does not belong to the subject '{request.subject}'."
                    });
                }
                var result = await kernelService.GenerateContentAsync(request);
                return TypedResults.Ok(result);
            })
            .WithName("GenerateContent");

            app.MapPost("/api/content/prompt", async Task<IResult> (PromptRequest request, KernelService kernelService, GuardrailService guardrailService, TopicValidationService topicValidationService) =>
            {
                var score = await guardrailService.GetPromptScoreAsync(request.Prompt);
                if(score < 7)
                {
                    return TypedResults.Ok(new PromptResponse
                    {
                        Message = "Sorry, I can only assist with educational content generation."
                    });
                }

                var subject = ExtractSubject(request.Prompt);
                var topic = ExtractTopic(request.Prompt);

                if(!string.IsNullOrWhiteSpace(subject) &&! string.IsNullOrWhiteSpace(topic))
                {
                    var topicScore = await topicValidationService.GetTopicScoreAsync (subject, topic);
                    if(topicScore < 7)
                    {
                        return TypedResults.Ok(new PromptResponse
                        {
                            Message = $"The topic '{topic}' does not belong to the subject '{subject}'."
                        });
                    }
                }

                var result = await kernelService.GeneratePromptContentAsync(request.Prompt);
                return TypedResults.Ok(result);
            })
            .WithName("GeneratePrompt");
        }

        private static string? ExtractSubject(string prompt)
        {
            var subjects = new[] { "physics", "chemistry", "biology", "math", "mathemtics", "history", "geography" };
            return subjects.FirstOrDefault(s => prompt.ToLower().Contains(s));
        }

        private static string? ExtractTopic(string prompt)
        {
            var lower = prompt.ToLower();
            if(lower.Contains("on"))
            {
                var parts = prompt.Split(" on ", StringSplitOptions.RemoveEmptyEntries);
                return parts.Last().Trim();
            }
            return null;
        }
    }
}
