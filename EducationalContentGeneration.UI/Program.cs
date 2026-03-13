using EducationalContentGeneration.UI.Components;
using EducationalContentGeneration.UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// register service Mock or Api
builder.Services.AddHttpClient<ContentApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:45581");
});

builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true;  });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
