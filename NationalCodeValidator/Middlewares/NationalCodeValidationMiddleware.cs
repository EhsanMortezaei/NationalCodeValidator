using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;

namespace NationalCodeValidator.Middlewares;

public class NationalCodeValidationMiddleware
{
    private readonly RequestDelegate _next;

    public NationalCodeValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/user") && context.Request.Method == HttpMethod.Post.ToString())
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);

            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(body))
                return;

            var json = JsonDocument.Parse(body);

            var hasValue = json.RootElement.TryGetProperty("nationalCode", out var codeElement);

            if (hasValue is false)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("کد ملی ارسال نشده است.");
                return;
            }

            var nationalCode = codeElement.GetString();
            var isValidateNationalCode = NationalCodeHelper.IsValidateNationalCode(nationalCode);
            if (isValidateNationalCode is false)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("کد ملی نامعتبر است.");
                return;
            }

            await context.Response.WriteAsync($"کد ملی معتبر است: {nationalCode}");
            return;
        }

        await _next(context);
    }
}

