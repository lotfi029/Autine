using Autine.Api;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

//app.MapPost("auth/login", async (HttpContext context, IAuthService authService) =>
//{
//    var request = await context.Request.ReadFromJsonAsync<LoginRequest>();

//    var result = await authService.GetTokenAsync(request!);

//    if (result.IsFailure)
//        return Results.BadRequest(result.Error);

//    return Results.Ok(result.Value);
//});

app.MapControllers();

app.Run();