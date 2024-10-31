using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class ChatGptQuizService
    {
        private readonly HttpClient _httpClient;

        public ChatGptQuizService()
        {
           _httpClient.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("(Bearer", "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");
        }

        public ChatGptQuizService(HttpClient httpClient)
        {
            _httpClient = httpClient;
           // _httpClient.DefaultRequestHeaders.Authorization
                        // = new AuthenticationHeaderValue("Authorization", "Bearer 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");
        }

        public async Task<string> GenerateQuiz(string skill)
        {
            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                new
                {
                    role = "system",
                    content = "You are a quiz generator."
                },
                new
                {
                    role = "user",
                    content = $"Create a quiz with 5 multiple-choice questions on the skill {skill}. Each question should have 4 options with one correct answer."
                }
            }
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API call failed with status code: {response.StatusCode}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Network error while communicating with ChatGPT: " + ex.Message);
            }
        }
    }
}
