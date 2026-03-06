using EducationalContentGeneration.API.Services;
using EducationalContentGeneration.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EducationalContentGeneration.API.Controllers
{
    [Route("api/content")]
    [ApiController]
    public class ContentPromptGenerationController : ControllerBase
    {
        private readonly KernelService _kernelService;

        public ContentPromptGenerationController(KernelService kernelService)
        {
            _kernelService = kernelService;
        }

        [HttpPost("prompt")]
        public async Task<IActionResult> Prompt([FromBody] PromptRequest request)
        {
            var result = await _kernelService.GeneratePromptContentAsync(request.Prompt);
            return Ok(result);
        }
    }
}
