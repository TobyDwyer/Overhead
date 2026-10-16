using Overhead.Api.Endpoints;
using Overhead.Api.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<OpenSkyClient>(client => client.BaseAddress = new Uri(builder.Configuration["OpenSky:BaseUrl"]!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapOpenSkyEndpoints();

app.Run();

