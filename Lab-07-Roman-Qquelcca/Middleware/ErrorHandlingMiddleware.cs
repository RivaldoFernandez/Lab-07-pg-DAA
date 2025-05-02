namespace Lab_07_Roman_Qquelcca.Middleware;

using Newtonsoft.Json;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Pasa la solicitud al siguiente middleware
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500; // Internal Server Error
            context.Response.ContentType = "application/json";
            var errorResponse = new { message = "Ocurrió un error interno", details = ex.Message };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse)); // Enviar la respuesta personalizada
        }
    }
}