using NationalCodeValidator;
using NationalCodeValidator.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseMiddleware<NationalCodeValidationMiddleware>();

app.MapGet("/", () => "اعتبار سنجی کد ملی .");


app.MapGet("/api/user/validate/{nationalCode}", (string nationalCode) =>
{
    var isValidateNationalCode = nationalCode.IsValidateNationalCode();
    if (isValidateNationalCode is false)
    {
        return Results.BadRequest("کد ملی نامعتبر است.");
    }
    return Results.Ok($"کد ملی معتبر است: {nationalCode}");
});

app.MapPost("/api/user/register", async (HttpContext context) =>
{
    
});

app.Run();
