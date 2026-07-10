using BookingSystem.Api.Configurations;
using BookingSystem.Infrastructure.Configurations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Api
builder.Services.AddApiConfiguration();

//Swagger
builder.Services.AddSwaggerConfiguration();

//Infrastructura
builder.Services.AddInfrastructure();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerConfiguration();
}

app.UseHttpsRedirection();

app.UseApplicationMiddleware();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
