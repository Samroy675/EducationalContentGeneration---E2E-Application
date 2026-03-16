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

        public async Task<List<McqQuestion>> GenerateMcqAsync(ContentGenerationRequest req)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/content/generate", req);

            var json = await response.Content.ReadAsStringAsync();

            if (json.Contains("message"))
            {
                var error = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                throw new Exception(error?["message"]);
            }

            if (!response.IsSuccessStatusCode) throw new Exception("API request failed");

            var result = JsonSerializer.Deserialize<McqResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.questions ?? new List<McqQuestion>();
        }

        public async Task<List<ShortAnswerQuestion>> GenerateShortAnswerAsync(ContentGenerationRequest req)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/content/generate", req);

            var json = await response.Content.ReadAsStringAsync();

            if (json.Contains("message"))
            {
                var error = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                throw new Exception(error?["message"]);
            }

            if (!response.IsSuccessStatusCode) throw new Exception("API request failed");

            var result = JsonSerializer.Deserialize<ShortAnswerResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Questions ?? new List<ShortAnswerQuestion>();
        }

        public async Task<List<LongAnswerQuestion>> GenerateLongAnswerAsync(ContentGenerationRequest req)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/content/generate", req);

            var json = await response.Content.ReadAsStringAsync();

            if (json.Contains("message"))
            {
                var error = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                throw new Exception(error?["message"]);
            }

            if (!response.IsSuccessStatusCode) throw new Exception("API request failed");

            var result = JsonSerializer.Deserialize<LongAnswerResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Questions ?? new List<LongAnswerQuestion>();
        }

        public async Task<ExplanationResponse> EvaluateAnswerAsync(ContentGenerationRequest req)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/content/generate", req);

            if (!response.IsSuccessStatusCode)
                throw new Exception("API request failed");

            var result = await response.Content.ReadFromJsonAsync<ExplanationResponse>();

            return result ?? new ExplanationResponse();
        }

        public async Task<QuestionPaperResponse> GenerateQuestionPaperAsync(ContentGenerationRequest req)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/content/generate", req);

            if (!response.IsSuccessStatusCode)
                throw new Exception("API request failed");

            var result = await response.Content.ReadFromJsonAsync<QuestionPaperResponse>();

            return result ?? new QuestionPaperResponse();
        }

        public async Task<PromptResponse> GeneratePromptAsync(PromptRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/content/prompt", request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("API request failed");

            var result = await response.Content.ReadFromJsonAsync<PromptResponse>();

            return result ?? new PromptResponse();
        }
    }
}
