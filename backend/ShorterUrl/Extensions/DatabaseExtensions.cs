using Microsoft.EntityFrameworkCore;
using ShorterUrl.Data;

namespace ShorterUrl.Extensions;

public static class DatabaseExtensions
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dataContext.Database.Migrate();
    }
}