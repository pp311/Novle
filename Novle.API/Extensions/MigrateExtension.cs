using Microsoft.EntityFrameworkCore;
using Novle.Infrastructure.Repositories.Base;

namespace Novel.API.Extensions;

public static class MigrateExtension
{
    public static async Task MigrateDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<NovleDbContext>();
        await dbContext.Database.MigrateAsync();
    } 
}