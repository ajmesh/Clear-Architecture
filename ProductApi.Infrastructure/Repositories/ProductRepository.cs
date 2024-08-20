using Microsoft.EntityFrameworkCore;
using ProductApi.Core.Entities;
using ProductApi.Core.Interfaces;

namespace ProductApi.Infrastructure.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
   public async Task<Product> GetByIdAsync(int id)
      => await context.Products.FindAsync(id);


   public async Task<IEnumerable<Product>> GetAllAsync()
      => await context.Products.ToListAsync();

   public async Task AddAsync(Product product)
   {
      await context.Products.AddAsync(product);
      await context.SaveChangesAsync();
   }

   public async Task UpdateAsync(Product product)
   {
      context.Products.Update(product);
      await context.SaveChangesAsync();
   }

   public async Task DeleteAsync(int id)
   {
      var product = await context.Products.FindAsync(id);
      if (product != null)
      {
         context.Products.Remove(product);
         await context.SaveChangesAsync();
      }
   }
}
