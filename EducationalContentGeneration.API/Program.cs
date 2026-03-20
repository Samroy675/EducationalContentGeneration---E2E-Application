using EducationalContentGeneration.API.Endpoints;
using EducationalContentGeneration.API.Services;
using EducationalContentGeneration.Core.Enums;
using EducationalContentGeneration.Core.Plugins;
using EducationalContentGeneration.Core.Prompting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.SemanticKernel;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EducationalContentGeneration.API",
        Version = "v1",
        Description = "Educational Content Generator API using Semantic Kernel"
    });

    c.MapType<ContentGenerationType>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(ContentGenerationType))
               .Select(n => new OpenApiString(n))
               .Cast<IOpenApiAny>()
               .ToList()
    });

    c.MapType<DifficultyLevel>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(DifficultyLevel))
               .Select(n => new OpenApiString(n))
               .Cast<IOpenApiAny>()
               .ToList()
    });

    c.MapType<EducationClass>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(EducationClass))
               .Select(n => new OpenApiString(n))
               .Cast<IOpenApiAny>()
               .ToList()
    });
});

builder.Services.AddScoped<IPromptLoader, FilePromptLoader>();
builder.Services.AddScoped<Kernel>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: config["AzureOpenAI:DeploymentName"],
        endpoint: config["AzureOpenAI:Endpoint"],
        apiKey: config["AzureOpenAI:ApiKey"]
    );

    return kernelBuilder.Build();
});

builder.Services.AddScoped<GuardrailService>();
builder.Services.AddScoped<KernelService>();
builder.Services.AddScoped<ContentGenerationPlugin>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EducationalContentGeneration.API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapContentEndpoints();
app.Run();