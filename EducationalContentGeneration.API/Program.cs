using EducationalContentGeneration.API.Services;
using EducationalContentGeneration.Core.Plugins;
using EducationalContentGeneration.Core.Prompting;
using Microsoft.OpenApi.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
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
});

// Prompt Loader (Scoped is correct if it eventually uses per-request items or DbContext)
builder.Services.AddScoped<IPromptLoader, FilePromptLoader>();

// Semantic Kernel 
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

// Kernel Service wrapper 
builder.Services.AddScoped<KernelService>();
builder.Services.AddScoped<ContentGenerationPlugin>();

// Ensure enums are serialized as strings in JSON responses
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen(c =>
{
    c.UseInlineDefinitionsForEnums();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EducationalContentGeneration.API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();