using FindBestProductAI.Models;
using Microsoft.Extensions.Logging;

namespace FindBestProductAI.Services;

public class CategoryService
{
    private readonly OpenAIService _openAIService;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(OpenAIService openAIService, ILogger<CategoryService> logger)
    {
        _openAIService = openAIService;
        _logger = logger;
    }

    public async Task<List<AttributeResponse>> ProcessCategoriesAsync(List<Category> categories)
    {
        var attributeResponses = new List<AttributeResponse>();

        foreach (var category in categories)
        {
            foreach (var subCategory in category.SubCategories)
            {
                var attributes = await _openAIService.GetPopularAttributesAsync(subCategory.CategoryId);
                if (attributes != null)
                {
                    attributeResponses.Add(new AttributeResponse
                    {
                        CategoryId = subCategory.CategoryId,
                        Attributes = attributes
                    });
                }
                else
                {
                    _logger.LogWarning($"No attributes returned for subcategory ID: {subCategory.CategoryId}");
                }
            }
        }

        _logger.LogInformation("Processed categories successfully.");
        return attributeResponses;
    }
}
