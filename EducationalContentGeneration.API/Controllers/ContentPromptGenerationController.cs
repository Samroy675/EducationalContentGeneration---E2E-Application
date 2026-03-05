using EducationalContentGeneration.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EducationalContentGeneration.API.Controllers
{
    [Route("api/content-prompt")]
    [ApiController]
    public class ContentPromptGenerationController : ControllerBase
    {
        private readonly KernelService _kernelService;

        public ContentPromptGenerationController(KernelService kernelService)
        {
            _kernelService = kernelService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate(string prompt)
        {
            var result = await _kernelService.GeneratePromptContentAsync(prompt);
            return Ok(result);
        }
    }
}
