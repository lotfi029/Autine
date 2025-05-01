using Autine.Api;
using Autine.Api.Hubs;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSignalR(options =>
{
    // Configure SignalR options if needed
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 102400; // 100 KB
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(
                "http://localhost:3000",
                "http://127.0.0.1:5500",
                "https://127.0.0.1:5500") // Added your client URLs
            .AllowCredentials();
    });
});
var app = builder.Build();


app.MapOpenApi();
app.MapScalarApiReference();


app.UseHttpsRedirection();

app.UseCors("ClientPermission");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.Run();