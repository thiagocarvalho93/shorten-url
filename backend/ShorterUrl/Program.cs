using Microsoft.EntityFrameworkCore;
using ShorterUrl.Data;
using ShorterUrl.Middlewares;
using ShorterUrl.Repository;
using ShorterUrl.Service;

var builder = WebApplication.CreateBuilder(args);

ConfigureMvc(builder);
ConfigureServices(builder);

var app = builder.Build();

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
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"Connection string: {connectionString}");

    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source=database.db"));
    builder.Services.AddTransient<ShortUrlRepository>();
    builder.Services.AddTransient<AnalyticsRepository>();
    builder.Services.AddTransient<ShortUrlService>();
}