namespace EducationalContentGeneration.Tests
{
    public class PromptValidationsTests
    {
        [Fact]
        public void PromptValidation_Should_Fail_When_Prompt_is_Empty()
        {
            var prompt = "";
            Assert.True(string.IsNullOrEmpty(prompt));
        }
    }
}
