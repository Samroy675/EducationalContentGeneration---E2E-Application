using System.Text;

namespace EducationalContentGeneration.Core.Prompting
{
    public class FilePromptLoader : IPromptLoader
    {
        public async Task<string> LoadAsync(string promptName)
        {
            if(string.IsNullOrWhiteSpace(promptName))
                throw new ArgumentException("Prompt name cannot be null.", nameof(promptName));

            var assembly = typeof(FilePromptLoader).Assembly;

            var resourceName = $"EducationalContentGeneration.Core.Prompts.{promptName}.prompt.txt";

            await using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                // Helpful debug: list what's actually embedded
                var available = string.Join("\n", assembly.GetManifestResourceNames());
                throw new FileNotFoundException(
                    $"Prompt '{promptName}' not found as an embedded resource. " +
                    $"Tried: '{resourceName}'.\nAvailable:\n{available}");
            }

            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            return await reader.ReadToEndAsync();
        }
    }
}