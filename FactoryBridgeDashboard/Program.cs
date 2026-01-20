using FactoryBridgeDashboard.Components;
using FactoryBridgeDashboard.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register our Industrial Service
builder.Services.AddSingleton<FactoryBridgeDashboard.Services.OpcUaService>();
builder.Services.AddHostedService<FactoryBridgeDashboard.Services.OrchestratorService>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<FactoryBridgeDashboard.Services.AlertService>();
builder.Services.AddSingleton<FactoryBridgeDashboard.Services.HistorianService>();
builder.Services.AddSingleton<FactoryBridgeDashboard.Services.ModbusService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
