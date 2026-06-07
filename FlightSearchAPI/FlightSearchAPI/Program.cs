using FlightSearchAPI.Data;
using FlightSearchAPI.Services;
using Microsoft.AspNetCore.Diagnostics;

var myAllowSpecificOrigins = "localhost:8080";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();


// Data context (loads JSON once)
builder.Services.AddSingleton<FlightDataContext>();

// Service layer
builder.Services.AddScoped<IFlightSearchService, FlightSearchService>();

// ✅ Logging (already included by default, but you can configure it)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:8080") // Your Vue app URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.WebHost.UseUrls("http://localhost:8080");
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(x=> x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(origin=> true));
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is KeyNotFoundException) // Example mapping for 404
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { error = "Resource not found." });
        }
        else if (exception is ArgumentException) // Example mapping for 400
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = exception.Message });
        }
    });
});

app.Run();
