using Mango.CouponApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.CouponApi.Repositories;

public class SeedDatabase
{
    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        try
        {
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            SeedCoupons(context);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedDatabase>>();
            logger.LogError(ex, "An error occured during migrate database");
            throw;
        }
    }

    private static void SeedCoupons(ApplicationDbContext context)
    {
        if (context.Coupons.Any()) return;

        var coupons = new List<Coupon> {
            new Coupon
            {
                CouponCode = "10OFF",
                DiscountAmount = 10
            },
            new Coupon
            {
                CouponCode = "20OFF",
                DiscountAmount = 20
            },
        };

        context.Coupons.AddRange(coupons);
        context.SaveChanges();
    }
}