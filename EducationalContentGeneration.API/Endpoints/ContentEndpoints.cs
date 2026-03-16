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
                   var result = await kernelService.GenerateContentAsync(request);
                return TypedResults.Ok(result);
            })
            .WithName("GenerateContent");

            app.MapPost("/api/content/prompt", async Task<IResult> (PromptRequest request, KernelService kernelService, GuardrailService guardrailService) =>
            {
                var score = await guardrailService.GetPromptScoreAsync(request.Prompt);
                if(score < 8)
                {
                    return TypedResults.Ok(new PromptResponse
                    {
                        Message = "Sorry, I can only assist with educational content generation."
                    });
                }
                var result = await kernelService.GeneratePromptContentAsync(request.Prompt);
                return TypedResults.Ok(result);
            })
            .WithName("GeneratePrompt");
        }
    }
}
