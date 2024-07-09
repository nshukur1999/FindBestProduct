using Microsoft.AspNetCore.Mvc;
namespace FindBestProductAI.Controllers;

[Route("/")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Welcome to the CategoryAttributesApp API. Please go http://localhost:5001/swagger/index.html asd use the /api/category endpoint to interact with the service.");
    }
}