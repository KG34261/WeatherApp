using Polly;
using Polly.Extensions.Http;
using System.Reflection;
using WeatherApp.Configurations;
using WeatherApp.Helpers;
using WeatherApp.Interface.Helpers;
using WeatherApp.Interface.Repository;
using WeatherApp.Interface.Service;
using WeatherApp.Repository;
using WeatherApp.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
builder.Services.AddSwaggerGen(c=>
{
    c.IncludeXmlComments(xmlPath);
});
builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("WeatherApi"));
builder.Services.AddHttpClient<IWeatherRepository, WeatherRepository>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetBreakerPolicy())
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)));
builder.Services.AddScoped<IRequestCounter, RequestCounter>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IWeatherService,WeatherService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
    HttpPolicyExtensions.HandleTransientHttpError().
    WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

static IAsyncPolicy<HttpResponseMessage> GetBreakerPolicy() =>
    HttpPolicyExtensions.HandleTransientHttpError().
    CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

