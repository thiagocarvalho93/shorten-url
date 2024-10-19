using Microsoft.EntityFrameworkCore;
using Serilog;
using ShorterUrl.Data;
using ShorterUrl.Extensions;
using ShorterUrl.Jobs;
using ShorterUrl.Middlewares;
using ShorterUrl.Repository;
using ShorterUrl.Service;

var builder = WebApplication.CreateBuilder(args);

ConfigureMvc(builder);
ConfigureServices(builder);

var app = builder.Build();

app.ApplyMigration();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthorization();
app.MapControllers();

app.Run();

void ConfigureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddMemoryCache();
    builder.Host.UseSerilog((context, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(context.Configuration));

    var connectionString = builder.Configuration.GetConnectionString("Sqlite");
    Console.WriteLine($"Connection string: {connectionString}");

    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
    builder.Services.AddTransient<ShortUrlRepository>();
    builder.Services.AddTransient<AnalyticsRepository>();
    builder.Services.AddTransient<ShortUrlService>();
    builder.Services.AddTransient<FakeDataService>();
    builder.Services.AddHostedService<GenerateFakeDataJob>();
}