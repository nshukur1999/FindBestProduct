using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FindBestProductAI.Models;
using Microsoft.Extensions.Logging;

public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<OpenAIService> _logger;

    public OpenAIService(HttpClient httpClient, string apiKey, ILogger<OpenAIService> logger)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        _logger = logger;
    }

    public async Task<List<string>> GetPopularAttributesAsync(int categoryId)
    {
        _logger.LogInformation($"Fetching popular attributes for category ID: {categoryId}");

        var requestContent = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "You are an assistant that provides popular attributes for product categories." },
                new { role = "user", content = $"Provide the 3 most popular attributes for category with ID {categoryId} in the following JSON format: {{ \"CategoryId\": {categoryId}, \"Attributes\": [\"attribute1\", \"attribute2\", \"attribute3\"] }}" }
            }
        };

        var requestBody = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        _logger.LogInformation($"Request JSON payload: {JsonSerializer.Serialize(requestContent)}");

        int retryCount = 0;
        int maxRetryAttempts = 5;
        double delay = 1.0;

        while (retryCount < maxRetryAttempts)
        {
            try
            {
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestBody);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Raw response: {responseString}");

                    try
                    {
                        var responseObject = JsonSerializer.Deserialize<OpenAIResponse>(responseString);
                        if (responseObject != null && responseObject.Choices != null && responseObject.Choices.Count > 0)
                        {
                            var jsonResponse = responseObject.Choices[0].Message.Content;
                            try
                            {
                                var attributeResponse = JsonSerializer.Deserialize<AttributeResponse>(jsonResponse);
                                if (attributeResponse != null && attributeResponse.Attributes != null && attributeResponse.Attributes.Count == 3)
                                {
                                    _logger.LogInformation($"Received attributes: {string.Join(", ", attributeResponse.Attributes)}");
                                    return attributeResponse.Attributes;
                                }
                                else
                                {
                                    _logger.LogError("AttributeResponse deserialization failed or the attributes count is not 3.");
                                }
                            }
                            catch (JsonException ex)
                            {
                                _logger.LogError($"Failed to deserialize jsonResponse to AttributeResponse. Error: {ex.Message}");
                                _logger.LogError($"jsonResponse content: {jsonResponse}");
                            }
                        }
                        else
                        {
                            _logger.LogError("Response object or Choices is null.");
                            _logger.LogError($"responseObject: {responseObject}");
                            if (responseObject != null)
                            {
                                _logger.LogError($"responseObject.Choices: {responseObject.Choices}");
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError($"Failed to deserialize responseString to OpenAIResponse. Error: {ex.Message}");
                        _logger.LogError($"responseString content: {responseString}");
                    }
                }
                else if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    retryCount++;
                    _logger.LogWarning($"Rate limit hit. Retrying in {delay} seconds...");
                    await Task.Delay(TimeSpan.FromSeconds(delay));
                    delay *= 2; // Exponential backoff
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogError("Access forbidden. Check your API key and permissions.");
                    throw new UnauthorizedAccessException("Access forbidden. Check your API key and permissions.");
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to fetch attributes for category ID: {categoryId}. Status code: {response.StatusCode}, Response: {errorResponse}");
                    return new List<string> { "Attribute1", "Attribute2", "Attribute3" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while fetching attributes: {ex.Message}");
            }
        }

        _logger.LogError("Max retry attempts exceeded. Failed to fetch attributes.");
        return new List<string> { "Attribute1", "Attribute2", "Attribute3" };
    }
}
