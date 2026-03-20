using EducationalContentGeneration.API.Services;
using EducationalContentGeneration.Core.Models;

namespace EducationalContentGeneration.API.Endpoints
{
    public static class ContentEndpoints
    {
        public static void MapContentEndpoints(this WebApplication app)
        {
            app.MapPost("/api/content/generate", async Task<IResult> (ContentGenerationRequest request, KernelService kernelService) => 
               {
                   if (request == null) return Results.BadRequest("Invalid request");

                   var result = await kernelService.GenerateContentAsync(request);
                return Results.Ok(result);
            })
            .WithName("GenerateContent");

            app.MapPost("/api/content/prompt", async Task<IResult> (PromptRequest request, KernelService kernelService, GuardrailService guardrailService) =>
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Prompt)) return Results.BadRequest("Prompt cannot be empty");

                var promptScore = await guardrailService.GetPromptScoreAsync(request.Prompt);

                if(promptScore < 8)
                {
                    return Results.BadRequest(new PromptResponse
                    {
                        Message = "Sorry, I can only assist with educational content generation."
                    });
                }
                var result = await kernelService.GeneratePromptContentAsync(request.Prompt);
                return Results.Ok(result);
            })
            .WithName("GeneratePrompt");
        }
    }
}
