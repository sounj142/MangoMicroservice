using Mango.OrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.OrderApi.Repositories;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<OrderHeader> OrderHeaders => Set<OrderHeader>();
    public DbSet<OrderDetails> OrderDetails => Set<OrderDetails>();
}