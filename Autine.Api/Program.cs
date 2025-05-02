using Autine.Api;
using Autine.Api.Hubs;
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
                "https://127.0.0.1:5500")
            .AllowCredentials();
    });
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "ChatApp:";
});

builder.Services.AddHybridCache();

var app = builder.Build();


app.MapOpenApi();
app.MapScalarApiReference();


app.UseHttpsRedirection();

app.UseCors("ClientPermission");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");
app.MapHub<DMChatHub>("/my-chat");

app.Run();