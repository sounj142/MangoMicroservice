using Mango.EmailSender.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.EmailSender.Repositories;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Email> Emails => Set<Email>();
}