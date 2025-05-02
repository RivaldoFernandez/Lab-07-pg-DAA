namespace Lab_07_Roman_Qquelcca.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class AdminTestController : ControllerBase
{
    [HttpGet("secure")]
    public IActionResult TestAdminAccess()
    {
        var role = User?.Claims?.FirstOrDefault(c => c.Type == "role")?.Value;
        return Ok(new { message = $"Bienvenido. Acceso permitido para el rol: {role}" });
    }
}
