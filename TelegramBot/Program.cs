using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using TelegramBot.Api.HostedServices;
using TelegramBot.Application;
using TelegramBot.Application.Options;
using TelegramBot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOptions();
services.AddControllers()
    .AddNewtonsoftJson();

services.AddDatabase(configuration);
services.AddApplicationServices();
services.AddInfrastructureServices();

services.Configure<TelegramBotOptions>(configuration.GetSection("TelegramBot"));

// Регистрация телеграм бота
builder.Services.AddHttpClient("TelegramBot")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    var telegramBotOptions = configuration.GetSection("TelegramBot").Get<TelegramBotOptions>();
                    TelegramBotClientOptions options = new(telegramBotOptions.Token);
                    return new TelegramBotClient(options, httpClient);
                });
services.AddHostedService<TelegramBotHostedService>();

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

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dataContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DataContext>>();
    var dataContext = dataContextFactory.CreateDbContext();
    dataContext.Database.MigrateAsync().GetAwaiter().GetResult();
}

app.Run();
