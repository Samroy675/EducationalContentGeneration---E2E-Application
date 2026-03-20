using EducationalContentGeneration.Core.Models;
using System.Text.Json;

namespace EducationalContentGeneration.UI.Services
{
    public class ContentApiService
    {
        private readonly HttpClient _httpClient;
        public ContentApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<McqQuestion>> GenerateMcqAsync(ContentGenerationRequest request)
        {
            var result = await PostAsync<McqResponse>("/api/content/generate", request);
            return result.Questions ?? new();
        }

        public async Task<List<ShortAnswerQuestion>> GenerateShortAnswerAsync(ContentGenerationRequest request)
        {
            var result = await PostAsync<ShortAnswerResponse>("/api/content/generate", request);
            return result.Questions ?? new();
        }

        public async Task<List<LongAnswerQuestion>> GenerateLongAnswerAsync(ContentGenerationRequest request)
        {
            var result = await PostAsync<LongAnswerResponse>("/api/content/generate", request);
            return result.Questions ?? new();
        }

        public async Task<ExplanationResponse> EvaluateAnswerAsync(ContentGenerationRequest request)
        {
            return await PostAsync<ExplanationResponse>("/api/content/generate", request);
        }

        public async Task<QuestionPaperResponse> GenerateQuestionPaperAsync(ContentGenerationRequest request)
        {
            return await PostAsync<QuestionPaperResponse>("/api/content/generate", request);
        }

        public async Task<PromptResponse> GeneratePromptAsync(PromptRequest req)
        {
            return await PostAsync<PromptResponse>("/api/content/prompt", req);
        }

        private async Task<T> PostAsync<T>(string url, object payload)
        {
            var response = await _httpClient.PostAsJsonAsync(url, payload);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                try
                {
                    var error = JsonSerializer.Deserialize<Dictionary<string, string>>(errorJson, options);
                    throw new HttpRequestException(error?["message"] ?? "API request failed");
                }
                catch
                {
                    throw new HttpRequestException("API request failed");
                }
            }

            var result = await response.Content.ReadFromJsonAsync<T>(options);

            return result ?? throw new InvalidOperationException("Empty response from API");
        }
    }
}
