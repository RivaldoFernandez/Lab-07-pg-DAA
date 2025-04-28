using Lab_07_Roman_Qquelcca.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Lab_07_Roman_Qquelcca.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductDto product)
    {
        return Ok(new { message = "Producto creado exitosamente." });
    }
}
