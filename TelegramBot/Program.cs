using Microsoft.EntityFrameworkCore;
using TelegramBot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOptions();
services.AddDatabase(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var dataContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DataContext>>();
    var dataContext = dataContextFactory.CreateDbContext();
    dataContext.Database.MigrateAsync().GetAwaiter().GetResult();
}

app.Run();
