using Microsoft.Extensions.Options;
using WeatherApi.Interfaces;
using WeatherApi.Models;
using WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Get OpenWeatherMap tokens in.
builder.Services.Configure<OpenWeatherMapConfig>(builder.Configuration.GetSection("OpenWeatherMap"));

// Get RateLimitConfig in, which contains ClientKeys and TokenCapacity.
builder.Services.Configure<RateLimitConfig>(builder.Configuration.GetSection("RateLimitConfig"));

// Initialize the Bucket & Tokens
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IOptions<RateLimitConfig>>().Value;
    return new TokenBucket(config.ClientKeys, config.TokenCapacity);
});

// Add services to the container.
builder.Services.AddSingleton<IRateLimitService, RateLimitService>();
builder.Services.AddTransient<IWeatherService, WeatherService>();
builder.Services.AddTransient<ILocationValidationService, LocationValidationService>();

// configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddControllers();

//Configure CORS
builder.Services.AddCors(options =>
{
    var allowedCorsOrigin = builder.Configuration.GetValue<string>("AllowedCorsOrigins") ?? "http://localhost:3000";
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins(allowedCorsOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
