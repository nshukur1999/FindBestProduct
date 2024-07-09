using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FindBestProductAI.Services;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(CategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessCategories([FromBody] List<Category> categories)
    {
        if (categories == null)
        {
            _logger.LogError("Categories list is null.");
            return BadRequest("Categories list cannot be null.");
        }

        var attributeResponses = await _categoryService.ProcessCategoriesAsync(categories);
        return Ok(attributeResponses);
    }
}