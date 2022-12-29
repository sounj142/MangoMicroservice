using Mango.ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.ShoppingCartApi.Repositories;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<CartHeader> CartHeaders => Set<CartHeader>();
    public DbSet<CartDetails> CartDetails => Set<CartDetails>();
}