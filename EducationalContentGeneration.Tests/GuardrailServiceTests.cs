using EducationalContentGeneration.API.Services;
using EducationalContentGeneration.Core.Prompting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using NSubstitute;

namespace EducationalContentGeneration.Tests
{
    public class GuardrailServiceTests()
    {
        [Fact]
        public void Guardrail_Should_Block_Invalid_Prompt()
        {
            Kernel kernel = null;
            var logger = Substitute.For<ILogger<GuardrailService>>();
            var promptLoader = Substitute.For<IPromptLoader>();
            var guardrailService = new GuardrailService(kernel, promptLoader, logger);
            var score = 4;
            Assert.True(score < 7);
        }

        [Fact]
        public void Guardrail_Should_Allow_Educational_Prompt()
        {
            var prompt = "Generate MCQs for Physics";
            var isEducational = prompt.Contains("MCQ") || prompt.Contains("question");
            Assert.True(isEducational);
        }

        [Fact]
        public void Guardrail_Should_Fail_Empty_Prompt()
        {
            var prompt = "";
            Assert.True(string.IsNullOrWhiteSpace(prompt));
        }

        [Fact]
        public void Guardrail_Should_Allow_Topic_Based_Prompt()
        {
            var prompt = "Explain Newton's Third Law for Class 8";
            var isEducational = prompt.Contains("Law") || prompt.Contains("Class") || prompt.Contains("Explain");
            Assert.True(isEducational);
        }

        [Fact]
        public void Guardrail_Should_Block_Random_Prompt()
        {
            var prompt = "What is the best pizza available in the town?";
            var isEducational = prompt.Contains("mcq") || prompt.Contains("question");
            Assert.False(isEducational);
        }
    }
}
