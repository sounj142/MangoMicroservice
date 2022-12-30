using Mango.CouponApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.CouponApi.Repositories;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Coupon> Coupons => Set<Coupon>();
}