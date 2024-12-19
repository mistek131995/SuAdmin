using Microsoft.EntityFrameworkCore;
using SuAdmin.Components;
using SuAdmin.Constants;
using SuAdmin.Extensions;
using SuAdmin.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

SQLitePCL.Batteries.Init();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<SqlLiteContext>(options =>
    options.UseSqlite(Common.ConnectionString));

await builder.Services.LoadPlugins();
//builder.Services.AddSingleton(plugins);

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var context = scope.ServiceProvider.GetRequiredService<SqlLiteContext>();
await context.Database.MigrateAsync();


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