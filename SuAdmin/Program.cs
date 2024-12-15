using Microsoft.EntityFrameworkCore;
using PluginContracts.Database;
using SuAdmin.Components;
using SuAdmin.Constants;
using SuAdmin.Extensions;

var builder = WebApplication.CreateBuilder(args);

SQLitePCL.Batteries.Init();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlite(Common.connectionString));

var plugins = builder.Services.LoadPlugins();
builder.Services.AddSingleton(plugins);

var app = builder.Build();

app.ConfigurePluginsDatabase();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();