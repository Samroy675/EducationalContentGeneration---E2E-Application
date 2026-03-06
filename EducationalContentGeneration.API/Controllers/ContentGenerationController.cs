using EducationalContentGeneration.API.Services;
using EducationalContentGeneration.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationalContentGeneration.API.Controllers
{
    [Route("api/content")]
    [ApiController]
    public class ContentGenerationController : ControllerBase
    {
        private readonly KernelService _kernelService;

        public ContentGenerationController(KernelService kernelService)
        {
            _kernelService = kernelService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] ContentGenerationRequest request)
        {
            var result = await _kernelService.GenerateContentAsync(request);
            return Ok(result);
        }
    }
}
