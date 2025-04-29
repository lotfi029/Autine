using Autine.Api;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();


app.MapOpenApi();
app.MapScalarApiReference();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();