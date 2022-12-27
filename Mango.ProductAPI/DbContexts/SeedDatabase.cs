using Mango.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.ProductAPI.DbContexts;

public class SeedDatabase
{
    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        try
        {
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            SeedCategories(context);
            SeedProducts(context);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedDatabase>>();
            logger.LogError(ex, "An error occured during migrate database");
            throw;
        }
    }

    private static void SeedCategories(ApplicationDbContext context)
    {
        if (context.Categories.Any()) return;

        var categories = new List<Category> {
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Appetizer"
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Dessert"
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Entree"
            },
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();
    }

    private static void SeedProducts(ApplicationDbContext context)
    {
        if (context.Products.Any()) return;
        var categories = context.Categories.OrderBy(x => x.Name).ToList();
        if (categories.Count < 3) return;

        var products = new List<Product> {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Samosa",
                Price = 15,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://hoangmangostorage.blob.core.windows.net/mango/14.jpg",
                Category = categories[0]
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Paneer Tikka",
                Price = 13.99,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://hoangmangostorage.blob.core.windows.net/mango/12.jpg",
                Category = categories[0]
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sweet Pie",
                Price = 10.99,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://hoangmangostorage.blob.core.windows.net/mango/11.jpg",
                Category = categories[1]
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Pav Bhaji",
                Price = 15,
                Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://hoangmangostorage.blob.core.windows.net/mango/13.jpg",
                Category = categories[2]
            },
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}