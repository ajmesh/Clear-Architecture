using Microsoft.EntityFrameworkCore;
using ProductApi.Core.Entities;

namespace ProductApi.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
   public DbSet<Product> Products { get; set; }
}
