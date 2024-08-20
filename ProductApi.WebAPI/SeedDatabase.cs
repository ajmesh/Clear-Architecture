using ProductApi.Core.Entities;
using ProductApi.Infrastructure;

namespace ProductApi.WebAPI;
public class SeedDatabase
{
   public void Seed(IApplicationBuilder app)
   {
      using (var serviceScope = app.ApplicationServices.CreateScope())
      {
         var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

         context.Database.EnsureCreated();

         if (!context.Products.Any())
         {
            context.Products.Add(new Product { Name = "Sample Product", Price = 9.99m });

            context.SaveChanges();
         }
      }
   }
}