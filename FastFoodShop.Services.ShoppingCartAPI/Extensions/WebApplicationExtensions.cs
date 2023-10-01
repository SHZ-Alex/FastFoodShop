using FastFoodShop.Services.ShoppingCartAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FastFoodShop.Services.ShoppingCartAPI.Extensions;

public static class WebApplicationExtensions
{
    public static void ApplyMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (db.Database.GetPendingMigrations().Any())
            db.Database.Migrate();
    }
}