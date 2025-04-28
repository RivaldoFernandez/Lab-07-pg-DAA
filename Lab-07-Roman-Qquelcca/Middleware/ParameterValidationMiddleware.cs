namespace Lab_07_Roman_Qquelcca.Middleware;

using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;

public class ParameterValidationMiddleware
{
    private readonly RequestDelegate _next;
    public ParameterValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
        {
            var model = await DeserializeRequestBody(context);
            if (model == null)
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Cuerpo de la solicitud inválido o vacío.");
                return;
            }
            var validationErrors = ValidateModel(model);
            if (validationErrors.Any())
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync(string.Join("\n", validationErrors));
                return;
            }
        }
        await _next(context);
    }
    private async Task<object> DeserializeRequestBody(HttpContext context)
    {
        var contentType = context.Request.ContentType;

        if (contentType.Contains("application/json"))
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var dtoType = context.GetEndpoint()?.Metadata?.OfType<ControllerActionDescriptor>()
                .FirstOrDefault()?.MethodInfo?.GetParameters().FirstOrDefault()?.ParameterType;

            if (dtoType != null)
            {
                var model = JsonConvert.DeserializeObject(body, dtoType);
                return model;
            }
        }
        return null;
    }
    private List<string> ValidateModel(object model)
    {
        var errors = new List<string>();
        var properties = model.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(model);
            if (value == null)
            {
                errors.Add($"El parámetro '{property.Name}' es obligatorio.");
            }
        }
        return errors;
    }
}
