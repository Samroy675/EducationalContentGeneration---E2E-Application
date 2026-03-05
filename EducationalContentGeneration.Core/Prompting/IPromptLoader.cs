namespace EducationalContentGeneration.Core.Prompting
{
    public interface IPromptLoader
    {
        Task<string> LoadAsync(string promptName);
    }
}
